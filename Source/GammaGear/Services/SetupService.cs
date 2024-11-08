using GammaGear.Models;
using GammaGear.Services.Contracts;
using GammaGear.Generated;
using Microsoft.VisualBasic.Logging;
using Python.Deployment;
//using Python.Included;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Microsoft.Extensions.Logging;

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

        private ILogger _logger = null;

        public SetupService(ILogger<SetupService> logger)
        {
            _logger = logger;

            _validInstallationPaths = new(_defaultInstallationPaths);

            foreach (var install in _validInstallationPaths)
            {
                if (!Path.Exists(install.Value))
                {
                    _validInstallationPaths.Remove(install.Key);
                }
            }
        }

        ~SetupService()
        {
            PythonEngine.Shutdown();
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


            if (!PythonEngine.IsInitialized)
            {
#if DEBUG
                // If we're in debug mode, delete the modules folder so we can upgrade the wheels
                DirectoryInfo modulesDirectory = new DirectoryInfo("modules");
                if (modulesDirectory.Exists)
                {
                    _logger.LogDebug("Deleting modules folder.");
                    modulesDirectory.Delete(true);
                }
#endif

                // Get executable location to install python to.
                FileInfo executableInfo = new FileInfo(Assembly.GetEntryAssembly().Location);
                DirectoryInfo executableDirectory = executableInfo.Directory;
                DirectoryInfo pythonInstallationDirectory = executableDirectory.CreateSubdirectory("modules" + Path.DirectorySeparatorChar + "python");
                if (!pythonInstallationDirectory.Exists)
                {
                    throw new DirectoryNotFoundException($"Could not install python to the \"{pythonInstallationDirectory.FullName}\" directory");
                }

                await SetupPython(pythonInstallationDirectory.FullName);

                Assembly assembly = Assembly.GetExecutingAssembly();
                string[] list = assembly.GetManifestResourceNames();
                foreach (string resourceName in list)
                {
                    if (resourceName.EndsWith(".whl"))
                    {
#if DEBUG
                        await Installer.InstallWheel(assembly, resourceName, true);
#else
                        await Installer.InstallWheel(assembly, resourceName, false);
#endif
                    }
                }

                //Runtime.PythonDLL = "python311.dll";
                PythonEngine.Initialize();
            }
            else
            {
                _logger.LogInformation("Python was already initialized");
            }

            PythonEngine.DebugGIL = true;
            using (Py.GIL())
            {

                try
                {
                    // Use wiztype to get types.json\
                    dynamic sys = Py.Import("sys");
                    Console.WriteLine("Python version: " + sys.version);
                    dynamic types = Py.Import("ggutils");
                    //types.get_types(_validInstallationPaths[installMode].ToPython(), "types.json");
                    types.read_types();
                }
                catch (Exception ex)
                {
                    // types.get_types (inconsistant) | WinError 6: The handle is invalid.
                    // types.read_types (Always) | The module has no attribute "read_types"
                    string exs = ex.Message;
                    _logger.LogError(ex, "Python commands returned an exception.");
                }
                //types.read_types();
            }

            System.Diagnostics.Debug.WriteLine("We made it to the end of the function!!!");
        }

        private static async Task SetupPython(string extractionDirectory)
        {
            Python.Runtime.Runtime.PythonDLL = PythonInfo.DllName;

            Python.Deployment.Installer.Source = new Python.Deployment.Installer.EmbeddedResourceInstallationSource
            {
                Assembly = Assembly.GetExecutingAssembly(),
                ResourceName = PythonInfo.ResourceName
            };
            Python.Deployment.Installer.PythonDirectoryName = extractionDirectory;
            Python.Deployment.Installer.InstallPath = extractionDirectory;
            await Python.Deployment.Installer.SetupPython(false);
        }
    }
}
