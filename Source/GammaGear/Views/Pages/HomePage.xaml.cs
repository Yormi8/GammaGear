using GammaGear.ViewModels.Pages;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.Ui.Controls;

namespace GammaGear.Views.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePageViewModel ViewModel { get; }
        private bool pageInitialized = false;

        public HomePage(HomePageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            Loaded += (_, _) =>
            {
                // For some reason this stuff doesn't update when bound to the viewmodel, so we have to set it up here
                // instead.

                if (!pageInitialized)
                {
                    CustomInstallButton.IsChecked = true;
                    CustomInstallButton.Command.Execute(CustomInstallButton.CommandParameter);

                    if (ViewModel.SteamInstallFound)
                    {
                        SteamInstallButton.IsChecked = true;
                        SteamInstallButton.Command.Execute(SteamInstallButton.CommandParameter);
                    }
                    if (ViewModel.NativeInstallFound)
                    {
                        NativeInstallButton.IsChecked = true;
                        NativeInstallButton.Command.Execute(NativeInstallButton.CommandParameter);
                    }
                    pageInitialized = true;
                }
            };

            InitializeComponent();
        }
    }
}
