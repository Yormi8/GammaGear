using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Wpf.Ui.Controls;
using Wpf.Ui;
using GammaGear.Services.Contracts;
using Wpf.Ui.Extensions;
using Microsoft.Extensions.Configuration;
using GammaGear.Services;
using GammaGear.Views.Pages;
using Microsoft.Extensions.Logging;
using GammaGear.ViewModels.Windows;

namespace GammaGear.Views
{
    /// <summary>
    /// Interaction logic for MasterWindow.xaml
    /// </summary>
    public partial class MainWindow : FluentWindow, IWindow
    {
        public MainWindowViewModel ViewModel { get; }
        public IAppearanceService AppearanceService { get; }
        public UserPreferencesService UserPrefs { get; }
        private ILogger _logger;

        public MainWindow(
            MainWindowViewModel viewModel,
            INavigationService navigationService,
            IServiceProvider serviceProvider,
            IAppearanceService appearanceService,
            UserPreferencesService prefs,
            ILogger<MainWindow> logger)
        {
            ViewModel = viewModel;
            DataContext = this;

            AppearanceService = appearanceService;
            UserPrefs = prefs;

            _logger = logger;

            InitializeComponent();

            navigationService.SetNavigationControl(RootNavigation);

            RootNavigation.SetServiceProvider(serviceProvider);

            Loaded += (_, _) => OnLoaded();

            App.Current.Startup += (_, _) => _logger.LogInformation("Application Startup event fired");
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Make sure that the application tries to quit when the main window is closed.
            Application.Current.Shutdown();
        }

        private void OnLoaded()
        {
            FixNavigationAppearance();
            ApplyThemeFromConfig();
        }

        private void FixNavigationAppearance()
        {
            // This gets overridden somewhere and needs to be set on startup instead...
            RootNavigation.IsPaneOpen = false;
            RootNavigation.FrameMargin = new Thickness(0, 0, 0, 0);
            RootNavigation.Navigate(typeof(HomePage));
        }

        private void ApplyThemeFromConfig()
        {
            AppearanceService.SetTheme(UserPrefs.Theme);
        }
    }
}
