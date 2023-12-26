using GammaGear.Models;
using GammaGear.Services.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            string rootWadPath = Path.Combine(_validInstallationPaths[installMode], "GameData", "Data", "Root.wad");

            if (!Path.Exists(rootWadPath))
            {
                reason = InstallationFailReason.RootWadNotFound;
                return false;
            }

            return true;
        }
    }
}
