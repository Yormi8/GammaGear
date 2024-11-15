using GammaGear.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GammaGear.Services.Contracts
{
    public interface IPythonService
    {
        IReadOnlyDictionary<InstallMode, string> GetAllInstallationPaths();
        bool CanCreateDatabase(InstallMode installMode, out InstallationFailReason reason);
        void CreateDatabase(InstallMode installMode);
        void DeserializeItems();
        void ExtractLocale();
    }
}
