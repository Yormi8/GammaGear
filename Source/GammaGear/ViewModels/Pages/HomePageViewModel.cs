using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Wpf.Ui.Input;
using Wpf.Ui.Controls;
using Wpf.Ui;

namespace GammaGear.ViewModels.Pages
{
    public class HomePageViewModel : ViewModelBase, INavigationAware
    {
        private readonly INavigationService _navigationService;

        public HomePageViewModel(INavigationService navigationService)
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
                    _navigationService.Navigate(typeof(Views.Pages.ItemsPage));
                    return;
            }
        }
    }
}
