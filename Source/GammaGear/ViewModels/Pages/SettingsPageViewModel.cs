using GammaGear.Models;
using GammaGear.Services;
using GammaGear.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui;

namespace GammaGear.ViewModels.Pages
{
    public class SettingsPageViewModel : ViewModelBase
    {
        private IAppearanceService _appearanceService;
        private UserPreferencesService _userPreferencesService;
        public ApplicationTheme Theme
        {
            get => _appearanceService.GetTheme();
            set
            {
                if (_appearanceService.GetTheme() != value)
                {
                    _appearanceService.SetTheme(value);
                    _userPreferencesService.Theme = value;
                    OnPropertyChanged(nameof(Theme));
                }
            }
        }

        public SettingsPageViewModel(
            IAppearanceService appearanceService,
            UserPreferencesService userPreferencesService)
        {
            _appearanceService = appearanceService;
            _userPreferencesService = userPreferencesService;
        }
    }
}
