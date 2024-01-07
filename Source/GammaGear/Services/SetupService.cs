using GammaGear.Models;
using GammaGear.Services.Contracts;
using Python.Included;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GammaGear.Services
{
    public class SetupService : ISetupService
    {
        private static readonly IReadOnlyDictionary<InstallMode, string> _defaultInstallationPaths = new Dictionary<InstallMode, string>()
        {
            { InstallMode.Native, Path.Combine("C:", "ProgramData", "KingsIsle Entertainment", "Wizard101") },
            { InstallMode.Steam, Path.Combine("C:", "Program Files (x86)", "Steam", "steamapps", "common", "Wizard101") },
            { InstallMode.Custom, Properties.UserPrefs.Default.RootPath },
        };
        private static readonly IEnumerable<string> _setupSteps = new List<string>()
        {
            "Process not yet started",
            "Checking for Root.wad",
            "Initializing Database",
            "Loading Root.wad into memory",
            "Copying item information into Database",
            "Loading character information into memory",
            "Copying character information into Database",
        }.AsReadOnly();
        private Dictionary<InstallMode, string> _validInstallationPaths;
        public SetupService()
        {
            _validInstallationPaths = new(_defaultInstallationPaths);

            foreach (var install in _validInstallationPaths)
            {
                if (!Path.Exists(install.Value))
                {
                    _validInstallationPaths.Remove(install.Key);
                }
            }
        }
        public IReadOnlyDictionary<InstallMode, string> GetAllInstallationPaths()
        {
            return _validInstallationPaths;
        }
        public bool CanCreateDatabase(InstallMode installMode, out InstallationFailReason reason)
        {
            reason = InstallationFailReason.None;

            if (!_validInstallationPaths.ContainsKey(installMode))
            {
                reason = InstallationFailReason.WizInstallFolderNotFound;
                return false;
            }

            string rootWadPath = Path.Combine(_validInstallationPaths[installMode], "Data", "GameData", "Root.wad");

            if (!Path.Exists(rootWadPath))
            {
                reason = InstallationFailReason.RootWadNotFound;
                return false;
            }

            return true;
        }

        public async Task CreateDatabaseAsync(InstallMode installMode)
        {
            if (!CanCreateDatabase(installMode, out _))
            {
                return;
            }
            await Installer.SetupPython();

            Assembly assembly = Assembly.GetExecutingAssembly();
            string[] list = assembly.GetManifestResourceNames();
            foreach (string resourceName in list)
            {
                if (resourceName.EndsWith(".whl"))
                {
                    await Installer.InstallWheel(assembly, resourceName);
                }
            }

            //Runtime.PythonDLL = "python311.dll";
            PythonEngine.Initialize();
            PythonEngine.DebugGIL = true;
            using (Py.GIL())
            {
                // Use wiztype to get types.json
                dynamic types = Py.Import("ggutils");

                types.get_types(_validInstallationPaths[installMode].ToPython(), "types.json");
            }
            PythonEngine.Shutdown();
        }
    }
}
