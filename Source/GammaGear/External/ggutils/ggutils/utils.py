import sys
import os
import time
import subprocess
import wiztype
from katsuba.op import *
from katsuba.utils import string_id
from katsuba.wad import Archive
import json
from pathlib import Path

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

def deserialize_all(install_folder: str, types_list: str, out_path: str, log_info):
    # Open a type list from file system
    type_list = TypeList.open(types_list)

    # Configure serializer options
    opts = SerializerOptions()
    opts.flags |= STATEFUL_FLAGS
    opts.shallow = False
    opts.skip_unknown_types = True
    #print(dir(opts))

    # Construct the serializer
    s = Serializer(opts, type_list)

    # Open an archive memory-mapped:
    a = Archive.mmap(install_folder + "/Data/GameData/Root.wad")
    log_info(f"{len(a)} files found in Root.wad")

    valid_ids = {
        string_id("class WizItemTemplate"): "WizItemTemplate",
        string_id("class ItemSetBonusTemplate"): "ItemSetBonusTemplate",
    }

    with open("types.json", "r") as j:
        data_types: dict = json.load(j)

    # With a glob pattern for filtering files:
    num_files = 0
    for path in a.iter_glob("ObjectData/**/*.xml"):
        data = a.deserialize(path, s)
        if data.type_hash in valid_ids:
            d = proprecurse(data, data_types)

            # If this is an item, check that it has stats before dumping
            if d["__type"] == "class WizItemTemplate" and \
                (d["m_equipEffects"] is None or \
                len(d["m_equipEffects"]) == 0):
                continue

            p = Path(out_path + "/" + path).parent
            p.mkdir(parents=True, exist_ok=True)
            if not os.path.exists(out_path + "/" + path.replace(".xml", ".json")):
                with open(out_path + "/" + path.replace(".xml", ".json"), "x") as f:
                    json.dump(d, f)
                    num_files += 1
    log_info(f"{num_files} files deserialized")

def extract_locale(install_folder: str, out_path: str, log_info):
    # Open an archive memory-mapped:
    a = Archive.mmap(install_folder + "/Data/GameData/Root.wad")

    # With a glob pattern for filtering files:
    for path in a.iter_glob("Locale/**/*.lang"):
        data = a[path]
        p = Path(out_path + "/" + path).parent
        p.mkdir(parents=True, exist_ok=True)
        with open(out_path + "/" + path, "xb") as f:
            f.write(data)

def proprecurse(act, data_types: dict) -> dict:

    if act is None:
        return None
    if str(act.__class__) == "<class 'katsuba.op.Vec3'>":
        return {
            "x": act.x,
            "y": act.y,
            "z": act.z
        }
    if str(act.__class__) == "<class 'katsuba.op.Color'>":
        return {
            "a": act.a,
            "b": act.b,
            "g": act.g,
            "r": act.r
        }

    valid_primitives = [
        "bool",
        "unsigned char",
        "char",
        "unsigned short",
        "short",
        "unsigned int",
        "int",
        "float",
        "double",
        "gid"
    ]
    item = dict()
    item["__type"] = data_types["classes"][str(act.type_hash)]["name"]
    for prop in data_types["classes"][str(act.type_hash)]["properties"]:
        #prop.key
        #print(data_types["classes"][str(act.type_hash)]["properties"][prop])
        if data_types["classes"][str(act.type_hash)]["properties"][prop]["container"] == "Static":
            if data_types["classes"][str(act.type_hash)]["properties"][prop]["type"] in valid_primitives:
                item[prop] = act.get(prop)
            elif data_types["classes"][str(act.type_hash)]["properties"][prop]["type"] == "std::string":
                item[prop] = str(act.get(prop) or "").removeprefix("b'").removesuffix("'")
            elif data_types["classes"][str(act.type_hash)]["properties"][prop]["type"].startswith("class"):
                item[prop] = proprecurse(act.get(prop), data_types)
            elif data_types["classes"][str(act.type_hash)]["properties"][prop]["type"].startswith("enum"):
                item[prop] = act.get(prop)
            else:
                log_info(f"deserializer: prop type was unexpected: {data_types["classes"][str(act.type_hash)]["properties"][prop]["type"]}")
        else:
            # container is a list-type structure
            item[prop] = []
            for p in act.get(prop):
                if data_types["classes"][str(act.type_hash)]["properties"][prop]["type"] in valid_primitives:
                    item[prop].append(p)
                elif data_types["classes"][str(act.type_hash)]["properties"][prop]["type"] == "std::string":
                    item[prop].append(str(p or "").removeprefix("b'").removesuffix("'"))
                elif data_types["classes"][str(act.type_hash)]["properties"][prop]["type"].startswith("class"):
                    item[prop].append(proprecurse(p, data_types))
                elif data_types["classes"][str(act.type_hash)]["properties"][prop]["type"].startswith("enum"):
                    item[prop] = act.get(prop)
                else:
                    log_info(f"deserializer: prop type was unexpected: {data_types["classes"][str(act.type_hash)]["properties"][prop]["type"]}")
    return item

def read_types(log_info):
    log_info("Hello world!")
