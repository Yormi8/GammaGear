using GammaGear.Models;
using GammaGear.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui;

namespace GammaGear.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private IAppearanceService _appearanceService;
        public ApplicationTheme Theme
        {
            get => _appearanceService.GetTheme();
            set
            {
                if (_appearanceService.GetTheme() != value)
                {
                    _appearanceService.SetTheme(value);
                    OnPropertyChanged(nameof(Theme));
                }
            }
        }

        public SettingsViewModel(
            IAppearanceService appearanceService)
        {
            _appearanceService = appearanceService;
        }
    }
}
