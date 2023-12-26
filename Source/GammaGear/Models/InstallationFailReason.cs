using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GammaGear.Models
{
    public enum InstallationFailReason
    {
        None = 0,
        RootWadNotFound = 1,
        WizInstallFolderNotFound = 2,
    }
}
