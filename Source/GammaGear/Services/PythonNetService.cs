using GammaGear.Models;
using GammaGear.Services.Contracts;
using GammaGear.Generated;
using Microsoft.VisualBasic.Logging;
using Python.Deployment;
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
using System.Windows.Controls;

namespace GammaGear.Services
{
    public sealed partial class PythonNetService : IPythonService, IDisposable
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

        private readonly ILogger _logger = null;

        public PythonNetService(ILogger<PythonNetService> logger)
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

            _ = InitializePython();
        }

        public void Dispose()
        {
            _logger.LogInformation("Checking if Python needs to be shutdown");
            if (PythonEngine.IsInitialized)
            {
                _logger.LogInformation("Shutting down Python");
                PythonEngine.Shutdown();
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

        public void DeserializeItems()
        {
            string types_location = "types.json";
            string out_path = "temp";
            if (!File.Exists(types_location))
            {
                CreateDatabase(InstallMode.Native);
            }

            using (Py.GIL())
            {
                // Use wiztype to get types.json\
                using (var scope = Py.CreateScope())
                {
                    // Create a python objects to pass our logger functions to python.
                    dynamic log_info = (new Action<string>(s => _logger.LogInformation("{s}", s))).ToPython();
                    dynamic log_error = (new Action<string>(s => _logger.LogError("{s}", s))).ToPython();
                    dynamic types = Py.Import("ggutils");
                    types.deserialize_all(_validInstallationPaths[InstallMode.Native].ToPython(), types_location, out_path, log_info);
                    //types.read_types(log_info);
                }
            }
        }

        public void CreateDatabase(InstallMode installMode)
        {
            if (!CanCreateDatabase(installMode, out _))
            {
                return;
            }

            using (Py.GIL())
            {
                try
                {
                    // Use wiztype to get types.json\
                    using (var scope = Py.CreateScope())
                    {
                        // Create a python objects to pass our logger functions to python.
                        dynamic log_info = (new Action<string>(s => _logger.LogInformation("{s}", s))).ToPython();
                        dynamic log_error = (new Action<string>(s => _logger.LogError("{s}", s))).ToPython();
                        dynamic types = Py.Import("ggutils");
                        string types_location = "types.json";
                        if (!File.Exists(types_location))
                        {
                            types.generate_types(_validInstallationPaths[installMode].ToPython(), types_location, log_info);
                        }
                        //types.read_types(log_info);
                    }
                }
                catch (Exception ex)
                {
                    string exs = ex.Message;
                    _logger.LogError(ex, "Python commands returned an exception.");
                }
            }
        }

        private async Task InitializePython()
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

#if DEBUG
            PythonEngine.DebugGIL = true;
#endif
            PythonEngine.BeginAllowThreads();
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
