using CommunityToolkit.Mvvm.Input;
using GammaGear.Extensions;
using GammaGear.ViewModels.Controls;
using GammaGear.Views.Pages;
using GammaItems;
using Microsoft.Extensions.Logging;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wpf.Ui;

namespace GammaGear.ViewModels.Pages
{
    public class LoadoutsPageViewModel : ViewModelBase
    {
        private readonly ObservableCollection<ItemLoadoutViewModel> _itemLoadouts;

        public IEnumerable<ItemLoadoutViewModel> ItemLoadouts => _itemLoadouts;
        public int ItemLoadoutCount => _itemLoadouts.Count;

        private ICommand _editLoadoutCommand;
        public ICommand EditLoadoutCommand => _editLoadoutCommand;
        private ICommand _newLoadoutCommand;
        public ICommand NewLoadoutCommand => _newLoadoutCommand;

        private readonly INavigationService _navigationService;
        private readonly ILogger<LoadoutsPageViewModel> _logger;

        public LoadoutsPageViewModel(ILogger<LoadoutsPageViewModel> logger, INavigationService navigationService)
        {
            _navigationService = navigationService;
            _logger = logger;

            _itemLoadouts = new ObservableCollection<ItemLoadoutViewModel>();

            for (int i = 0; i < 100; i++)
            {
                _itemLoadouts.Add(new ItemLoadoutViewModel(GammaExtensions.GenerateRandomLoadout()));
            }

            _editLoadoutCommand = new RelayCommand<ItemLoadoutViewModel>(EditLoadout);
            _newLoadoutCommand = new RelayCommand(NewLoadout);
        }

        private void EditLoadout(ItemLoadoutViewModel loadout)
        {
            _logger.LogDebug("Navigate to Edit Loadout ({})", loadout.Name);
            _navigationService.NavigateWithHierarchy(typeof(LoadoutEditPage), loadout);
        }

        private void NewLoadout()
        {
            _logger.LogDebug("Create new loadout");
        }
    }
}
