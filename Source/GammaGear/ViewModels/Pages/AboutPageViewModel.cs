using ABI.System;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GammaGear.ViewModels.Pages
{
    public class AboutPageViewModel : ViewModelBase
    {
        public string VersionInfo => ApplicationInfo.AppDisplayVersion;
        private ICommand _navigateWebLinkCommand;
        public ICommand NavigateWebLinkCommand => _navigateWebLinkCommand;
        public AboutPageViewModel()
        {
            _navigateWebLinkCommand = new RelayCommand<string>(NavigateWebLink);
        }
        private void NavigateWebLink(string url)
        {
            System.Diagnostics.Process.Start(
                new System.Diagnostics.ProcessStartInfo(new System.Uri(url).AbsoluteUri)
                {
                    UseShellExecute = true
                }
            );
        }
    }
}
