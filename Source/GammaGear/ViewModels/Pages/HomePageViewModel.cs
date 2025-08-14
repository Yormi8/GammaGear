using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Wpf.Ui.Controls;
using Wpf.Ui;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.Input;
using GammaGear.Services.Contracts;
using GammaGear.Models;
using GammaItems.Source.Database;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.FileSystemGlobbing;
using System.Collections.ObjectModel;
using GammaGear.ViewModels.Controls;

namespace GammaGear.ViewModels.Pages
{
    public class HomePageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IPythonService _pythonService;
        private readonly ILogger _logger;
        private readonly ItemsPageViewModel _itemsPageViewModel;
        private bool _nativeInstallFound = true;
        public bool NativeInstallFound
        {
            get => _nativeInstallFound;
            set
            {
                if (_nativeInstallFound != value)
                {
                    _nativeInstallFound = value;
                    OnPropertyChanged(nameof(NativeInstallFound));
                }
            }
        }
        private bool _steamInstallFound = true;
        public bool SteamInstallFound
        {
            get => _steamInstallFound;
            set
            {
                if (_steamInstallFound != value)
                {
                    _steamInstallFound = value;
                    OnPropertyChanged(nameof(SteamInstallFound));
                }
            }
        }
        public InstallMode InstallMode { get; set; }

        public HomePageViewModel(
            INavigationService navigationService,
            IPythonService setupService,
            ILogger<HomePageViewModel> logger,
            ItemsPageViewModel itemsPageViewModel)
        {
            _navigationService = navigationService;
            _pythonService = setupService;
            _logger = logger;
            _itemsPageViewModel = itemsPageViewModel;

            var installLocations = _pythonService.GetAllInstallationPaths();
            NativeInstallFound = installLocations.ContainsKey(InstallMode.Native);
            SteamInstallFound = installLocations.ContainsKey(InstallMode.Steam);
        }

        private IRelayCommand _finishSetupCommand;
        private IRelayCommand _createDatabaseCommand;
        private IRelayCommand _changeInstallModeCommand;
        public IRelayCommand FinishSetupCommand => _finishSetupCommand ??= new RelayCommand(FinishSetup, CanFinishSetup);
        public IRelayCommand CreateDatabaseCommand => _createDatabaseCommand ??= new RelayCommand(CreateDatabaseAsync, CanCreateDatabase);
        public IRelayCommand ChangeInstallModeCommand => _changeInstallModeCommand ??= new RelayCommand<string>(ChangeInstallMode);

        private void FinishSetup()
        {

        }

        private bool CanFinishSetup()
        {
            return false;
        }

        private async void CreateDatabaseAsync()
        {
            _logger.LogInformation("Create Database started");
            await CreateDatabaseWorkAsync();
            _logger.LogInformation("Create Database finished");
        }

        internal async Task CreateDatabaseWorkAsync()
        {
            await Task.Run(() => {
                _logger.LogInformation("Creating types.json if it doesn't exist");
                _pythonService.CreateDatabase(InstallMode);
                _logger.LogInformation("Deserializing items if they don't exist");
                _pythonService.DeserializeItems();
                _logger.LogInformation("Extracting locale if it doesn't exist");
                _pythonService.ExtractLocale();

                string localePath = "temp/Locale/English";

                Matcher matcher = new Matcher();
                matcher.AddInclude("**/*.json");
                var files = matcher.GetResultsInFullPath("temp");
                _logger.LogInformation("Loading locale");
                KiJsonParser<KiTextLocaleBank> parser = new(localePath);
                _logger.LogInformation("Loading items");
                var collection = parser.ReadAllToItemBase(files);
                IEnumerable<GammaItems.Item> items = from o in collection where o is GammaItems.Item select o as GammaItems.Item;
                ObservableCollection<ItemViewModel> itemViewModels = new ObservableCollection<ItemViewModel>();
                foreach (var item in items)
                {
                    itemViewModels.Add(new ItemViewModel(item));
                }
                App.Current.Dispatcher.Invoke(() =>
                {
                    _itemsPageViewModel.OverrideItems(itemViewModels);
                });
            });
        }

        private bool CanCreateDatabase()
        {
            return _pythonService.CanCreateDatabase(InstallMode, out _);
        }

        private void ChangeInstallMode(string installMode)
        {
            System.Diagnostics.Debug.WriteLine($"Install mode now {installMode}");
            InstallMode = installMode switch
            {
                "native" => InstallMode.Native,
                "steam" => InstallMode.Steam,
                "custom" => InstallMode.Custom,
                _ => throw new ArgumentException($"{installMode} is not a valid install mode", nameof(installMode)),
            };
            CreateDatabaseCommand.NotifyCanExecuteChanged();
        }
    }
}
