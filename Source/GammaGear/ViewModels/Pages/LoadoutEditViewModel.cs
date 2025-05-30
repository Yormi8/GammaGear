using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GammaGear.Extensions;
using GammaGear.Views.Pages;
using GammaItems;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wpf.Ui;

namespace GammaGear.ViewModels.Pages
{
    public partial class LoadoutEditViewModel : ViewModelBase
    {
        public ItemLoadoutViewModel CurrentLoadout { get; set; }
        private string title;
        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
        private ILogger<LoadoutEditViewModel> _logger;
        private INavigationService _navigationService;
        public LoadoutEditViewModel(ILogger<LoadoutEditViewModel> logger, INavigationService navigationService)
        {
            _logger = logger;
            _navigationService = navigationService;
        }

        [RelayCommand]
        public void CancelClick()
        {
            _navigationService.GoBack();
        }
    }
}
