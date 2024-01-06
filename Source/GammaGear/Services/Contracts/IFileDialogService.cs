using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GammaGear.Services.Contracts
{
    public interface IFileDialogService
    {
        public string OpenFileDialog();
        public string OpenFolderDialog();
    }
}
