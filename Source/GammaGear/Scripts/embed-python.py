import sys
import argparse
import tomllib
from os.path import exists
from os.path import join as path_join
from subprocess import run
import re
import textwrap
import urllib.request
import hashlib
from zipfile import ZipFile

g_pyversion = ''
g_pyfilename = ''
g_pytargetdir = ''
g_pydllname = ''
g_genfile = ''

def main() -> int:
    return_code = embed_python()
    if return_code > 0:
        return return_code
    else:
        print(path_join(g_pytargetdir, g_pyfilename))
        with ZipFile(path_join(g_pytargetdir, g_pyfilename), 'r') as f:
            files = f.namelist()
        target_files = [file for file in files if file.endswith('.dll') and file.startswith('python')]
        g_pydllname = max(target_files, key=len)

        # Generate C# file from template
        content = ''
        with open(g_genfile, 'r') as f:
            content = f.read()
            content = re.sub('###PYVERSION###', g_pyversion, content)
            content = re.sub('###PYRESOURCE###', g_pyfilename, content)
            content = re.sub('###PYDLLNAME###', g_pydllname, content)

        generated_file = f"{g_genfile.removesuffix('.t.cs')}.g.cs"
        with open(generated_file, 'w') as f:
            f.write(content)

    return 0


def cmp(a, b):
    return (a > b) - (a < b)

def embed_python() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument(
        'requirements_file',
        help="A pyproject.toml file containing valid python versions."
    )
    parser.add_argument('out_dir', default='.')
    parser.add_argument('gen_file')

    parser.add_argument('-o', '--oldest',
        action='store_true',
        help="If this flag is present, get the oldest python version in the set constraints instead of the latest")

    args = parser.parse_args()

    global g_pyversion
    global g_pytargetdir
    global g_pyfilename
    global g_genfile

    if not exists(args.requirements_file):
        print("Error: Requirements file not found.")
        return 1

    if not exists(args.gen_file):
        print("Error: C# generation template not found.")
        return 1

    g_genfile = args.gen_file

    with open(args.requirements_file, 'rb') as f:
        data = tomllib.load(f)
        f.seek(0)
        hash_func = hashlib.new("sha1")
        hash_func.update(f.read())
        data_hash = hash_func.hexdigest()

    lock_file_path = path_join(args.out_dir, 'python.lock')
    if exists(lock_file_path):
        with open(lock_file_path, 'rb') as f:
            lock_version = f.readline().decode("utf-8").strip()
            lock_hash = f.readline().decode("utf-8").strip()

        current_py_installation_path = path_join(args.out_dir, f"python-{lock_version}-embed-amd64.zip")
        if exists(current_py_installation_path) and lock_hash == data_hash:
            print("Skip: Python installation matching lock found.")

            g_pyversion = lock_version
            g_pytargetdir= args.out_dir
            g_pyfilename= f"python-{lock_version}-embed-amd64.zip"
            return -1

    minpyver = ''
    minpyverstrat = 0
    maxpyver = ''
    maxpyverstrat = 0
    pyconstraintstring = ''

    # TODO: Maybe take in multiple values for the requirements file and try to find a
    #       suitable version matching all constraints?

    # Try and get python constraint from pyproject.toml
    # TODO: Implement all dep version strategies? (caret, tilde, wildcard)
    try:
        py_strings_unformat = data['project']['requires-python'].split(',')
        pyconstraintstring = data['project']['requires-python']

        for py_string in py_strings_unformat:
            py_string = py_string.replace(' ', '')
            #print(py_string)
            while not py_string[0].isdigit():

                # Get the sign of the expression (1 or 2 characters)
                sign = py_string[:2]

                # If our second character starts the version, remove it from our sign
                if sign[1].isdigit():
                    sign = sign[0]

                # Separate the sign and version
                py_string = py_string.removeprefix(sign)


                if sign == '<':
                    maxpyver = py_string
                    maxpyverstrat = -1
                elif sign == '<=':
                    maxpyver = py_string
                    maxpyverstrat = 0
                elif sign == '>':
                    minpyver = py_string
                    minpyverstrat = 1
                elif sign == '>=':
                    minpyver = py_string
                    minpyverstrat = 0

    except KeyError:
        pass

    # TODO: Implement other build systems' ways for versioning python?
    #print(f"minpyver {minpyver}")
    #print(f"minpyverstrat {minpyverstrat}")
    #print(f"maxpyver {maxpyver}")
    #print(f"maxpyverstrat {maxpyverstrat}")
    print(f"Python constraint: python = \"{pyconstraintstring}\"")

    if maxpyver == '' and minpyver == '':
        print(f"Error: Could not find valid python constraints")
        return 1

    minpyversplit = minpyver.split('.')
    minpy = int(''.join([char.zfill(4) for char in minpyversplit]).ljust(12, '0'))

    maxpyversplit = maxpyver.split('.')
    maxpy = int(''.join([char.zfill(4) for char in maxpyversplit]).ljust(12, '0'))

    #print(f"minpy {minpy}")
    #print(f"maxpy {maxpy}")

    gitoutput = run(['git', 'ls-remote', '--tags', '--refs', 'https://github.com/python/cpython', 'refs/tags/v*'], capture_output=True).stdout.decode("utf-8")
    gitoutput2 = sorted(set(re.findall(r"(?:\d+\.)+\d+(?!\.)\b", gitoutput)))
    gitoutput3 = sorted([int(''.join([char.zfill(4) for char in version.split('.')]).ljust(12, '0')) for version in gitoutput2])

    supported_versions = [version for version in gitoutput3 if
        cmp(version, minpy) >= minpyverstrat and
        cmp(version, maxpy) <= maxpyverstrat
    ]

    #print(supported_versions)

    selected_version = 0

    # TODO: Check if python.lock's version is compatible with new requirements.

    if args.oldest:
        selected_version = supported_versions[0]
    else:
        selected_version = supported_versions[-1]

    selected_version_string = '.'.join([chunk[::-1].lstrip('0') for chunk in textwrap.wrap(str(selected_version)[::-1], 4)[::-1]])
    #print(f"Selected version: {selected_version}")

    ftp_file = f"python-{selected_version_string}-embed-amd64.zip"
    local_file = path_join(args.out_dir, ftp_file)
    dl_link = f"https://www.python.org/ftp/python/{selected_version_string}/{ftp_file}"

    print(f"Selected python version: {selected_version_string}: {dl_link}")

    #local_filename, headers = urllib.request.urlretrieve(dl_link, local_file)

    resp = urllib.request.urlopen(dl_link).read()
    with open(local_file, 'wb') as f:
        f.write(resp)

    lock_file_path = path_join(args.out_dir, "python.lock")

    with open(lock_file_path, 'w') as f:
        f.write(f'{selected_version_string}\n')
        f.write(f'{data_hash}\n')

    g_pyversion = selected_version_string
    g_pytargetdir = args.out_dir
    g_pyfilename = f"python-{selected_version_string}-embed-amd64.zip"

    return 0

if __name__ == "__main__":
    sys.exit(main())
