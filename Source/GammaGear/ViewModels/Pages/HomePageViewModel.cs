﻿using System;
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

namespace GammaGear.ViewModels.Pages
{
    public class HomePageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ISetupService _setupService;
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
            ISetupService setupService)
        {
            _navigationService = navigationService;
            _setupService = setupService;

            var installLocations = _setupService.GetAllInstallationPaths();
            NativeInstallFound = installLocations.ContainsKey(InstallMode.Native);
            SteamInstallFound = installLocations.ContainsKey(InstallMode.Steam);
        }

        private IRelayCommand _finishSetupCommand;
        private IRelayCommand _createDatabaseCommand;
        private IRelayCommand _changeInstallModeCommand;
        public IRelayCommand FinishSetupCommand => _finishSetupCommand ??= new RelayCommand(FinishSetup, CanFinishSetup);
        public IRelayCommand CreateDatabaseCommand => _createDatabaseCommand ??= new RelayCommand(CreateDatabase, CanCreateDatabase);
        public IRelayCommand ChangeInstallModeCommand => _changeInstallModeCommand ??= new RelayCommand<string>(ChangeInstallMode);

        private void FinishSetup()
        {

        }

        private bool CanFinishSetup()
        {
            return false;
        }

        private void CreateDatabase()
        {
            System.Diagnostics.Debug.WriteLine($"Create Database called");
            _setupService.CreateDatabaseAsync(InstallMode);
        }

        private bool CanCreateDatabase()
        {
            return _setupService.CanCreateDatabase(InstallMode, out _);
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
