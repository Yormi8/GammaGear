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
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace GammaGear.Views
{
    /// <summary>
    /// Interaction logic for MasterWindow.xaml
    /// </summary>
    public partial class MainWindow : UiWindow, INavigationWindow
    {
        public MainViewModel ViewModel { get; }

        public MainWindow(
            MainViewModel viewModel,
            INavigationService navigationService,
            IPageService pageService)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();

            SetPageService(pageService);

            navigationService.SetNavigationControl(RootNavigation);
        }

        public void CloseWindow() => Close();

        public Frame GetFrame() => RootFrame;

        public INavigation GetNavigation() => RootNavigation;

        public bool Navigate(Type pageType) => RootNavigation.Navigate(pageType);

        public void SetPageService(IPageService pageService) => RootNavigation.PageService = pageService;

        public void ShowWindow() => Show();

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Make sure that the application tries to quit when the main window is closed.
            Application.Current.Shutdown();
        }


    }
}
