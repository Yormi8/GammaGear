import sys
import os
import time
import subprocess
import wiztype
from katsuba.op import *

def get_types(install_folder: str, output_file: str) -> str:
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
            print("Waiting for W101 to be open")
            time.sleep(2)

    dumper = dump_type(type_tree)
    dumper.dump(output_file)
    process.terminate()

    return "types.json created"

def read_types():
    type_list = None
    type_list = TypeList.open("types.json")
    print(type_list)

    return "types.json read"