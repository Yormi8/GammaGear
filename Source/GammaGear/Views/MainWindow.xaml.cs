using GammaGear.ViewModels;
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

namespace GammaGear.Views
{
    /// <summary>
    /// Interaction logic for MasterWindow.xaml
    /// </summary>
    public partial class MainWindow : FluentWindow, IWindow
    {
        public MainViewModel ViewModel { get; }

        public MainWindow(
            MainViewModel viewModel,
            INavigationService navigationService,
            IServiceProvider serviceProvider)
        {
            ViewModel = viewModel;
            DataContext = this;


            InitializeComponent();

            navigationService.SetNavigationControl(RootNavigation);

            RootNavigation.SetServiceProvider(serviceProvider);

            Loaded += (_, _) => OnLoaded();
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
            SyncToSystemTheme();
        }

        private void FixNavigationAppearance()
        {
            // This gets overridden somewhere and needs to be set on startup instead...
            RootNavigation.IsPaneOpen = false;
            RootNavigation.FrameMargin = new Thickness(-5, 0, 0, 0);
        }

        private void SyncToSystemTheme()
        {
            Wpf.Ui.Appearance.ApplicationThemeManager.ApplySystemTheme(true);
            Wpf.Ui.Appearance.SystemThemeWatcher.Watch(
                this,                    // Window class
                WindowBackdropType.Mica, // Background type
                true                     // Whether to change accents automatically
            );
        }
    }
}
