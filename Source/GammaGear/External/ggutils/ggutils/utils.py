import sys
import os
import time
import subprocess
import wiztype
from katsuba.op import *
from katsuba.utils import string_id

def generate_types(install_folder: str, output_file: str, log_info):
    clientOpen = False
    type_tree = None
    dump_type = wiztype.JsonTypeDumperV2

    info = subprocess.STARTUPINFO()
    info.dwflags = subprocess.STARTF_USESHOWWINDOW
    #info.wShowWindow = 0 # Hidden window
    info.wShowWindow = 2 # Minimized window

    # Opens the graphical client without the user having to login.
    process = subprocess.Popen([os.path.join(install_folder, "Bin", "WizardGraphicalClient.exe"), "-L", "login.us.wizard101.com 12000"],
                               cwd = os.path.join(install_folder, "Bin"),
                               startupinfo = info)

    # Avoid a crash with a sleep!
    time.sleep(2)
    while (not clientOpen):
        try:
            type_tree = wiztype.get_type_tree()
            clientOpen = True
        except ValueError as e:
            #print("Waiting for W101 to be open")
            log_info("Waiting for W101 to be opened")
            time.sleep(2)

    dumper = dump_type(type_tree)
    dumper.dump(output_file)
    process.terminate()

    ## Open a type list from file system
    #type_list = TypeList.open("types.json")
    #
    ## Configure serializer options
    #opts = SerializerOptions()
    #opts.flags |= STATEFUL_FLAGS
    #opts.shallow = False
    #
    ## Construct the serializer
    #ser = Serializer(opts, type_list)
    #
    ## Deserialize a file
    #with open("TemplateManifest.xml", "rb") as f:
    #    manifest = f.read()
    #    assert manifest[:4] == b"BINd"
    #
    #manifest = ser.deserialize(manifest[4:])
    #
    ## Make sure we deserialized the right object:
    #assert manifest.type_hash == string_id("class TemplateManifest")
    #
    ## Iterate the templates in the resulting object:
    #with open("TemplateManifest.xml.de.txt", 'w') as f:
    #    for location in manifest["m_serializedTemplates"]:
    #        f.write(f"Template {location['m_id']} at {location['m_filename']}")

def read_types(log_info):
    log_info("Hello world!")
