using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Common.Interfaces;
using Wpf.Ui.Input;

namespace GammaGear.ViewModels
{
    public class HomeViewModel : ViewModelBase, INavigationAware
    {
        private readonly INavigationService _navigationService;

        public HomeViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        private ICommand _navigateCommand;
        public ICommand NavigateCommand => _navigateCommand ??= new RelayCommand<string>(OnNavigate);

        public void OnNavigatedFrom()
        {
            // TODO: Logging
        }

        public void OnNavigatedTo()
        {
            // TODO: Logging
        }

        private void OnNavigate(string parameter)
        {
            switch (parameter)
            {
                case "navigate_to_items":
                    _navigationService.Navigate(typeof(Views.Pages.Items));
                    return;
            }
        }
    }
}
