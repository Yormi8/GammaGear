using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GammaGear.Extensions;
using GammaGear.ViewModels.Controls;
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
    public partial class LoadoutEditViewModel(ILogger<LoadoutEditViewModel> logger, INavigationService navigationService) : ViewModelBase
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

        [RelayCommand]
        public void CancelClick()
        {
            navigationService.GoBack();
        }
    }
}
