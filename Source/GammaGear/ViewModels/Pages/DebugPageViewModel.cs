using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using GammaGear.Services.Contracts;
using GammaGear.Logging;
using CommunityToolkit.Mvvm.Input;
using GammaGear.Services;
using GammaGear.Views.Windows;

namespace GammaGear.ViewModels.Pages
{
    public partial class DebugPageViewModel : ViewModelBase
    {
        private readonly WindowsProviderService _windowsProviderService;
        public ObservableCollection<Log> LogMessages => _logViewService.LogMessages;
        private readonly ILogViewService _logViewService;
        private ILogger _logger;
        private int _fontSize = 14;
        public int FontSize
        {
            get => _fontSize;
            set
            {
                _fontSize = value;
                OnPropertyChanged(nameof(FontSize));
            }
        }

        public DebugPageViewModel(ILogger<DebugPageViewModel> logger, ILogViewService logViewService, WindowsProviderService windowsProviderService)
        {
            _logger = logger;
            _logViewService = logViewService;
            _windowsProviderService = windowsProviderService;
        }

        [RelayCommand]
        public void OnOpenWindow()
        {
            _windowsProviderService.Show<SandboxWindow>();
        }
    }
}
