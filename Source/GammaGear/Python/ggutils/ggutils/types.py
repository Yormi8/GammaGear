import sys
import os
import time
import subprocess
import wiztype

def get_types(install_folder: str, output_file: str):
    clientOpen = False
    type_tree = None
    dump_type = wiztype.JsonTypeDumperV2

    # Opens the graphical client without the user having to login.
    process = subprocess.Popen([os.path.join(install_folder, "Bin", "WizardGraphicalClient.exe"), "-L", "login.us.wizard101.com 12000"],
                               cwd = os.path.join(install_folder, "Bin"),
                               )

    # Avoid a crash with a sleep!
    time.sleep(2)
    while (not clientOpen):
        try:
            type_tree = wiztype.get_type_tree()
            clientOpen = True
        except ValueError as e:
            print("Waiting for W101 to be open")
            time.sleep(2)

    process.terminate()

    dumper = dump_type(type_tree)
    dumper.dump(output_file, 0)
