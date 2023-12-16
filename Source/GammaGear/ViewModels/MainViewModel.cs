using GammaGear.Views.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.Networking.Sockets;
using Windows.Services.Maps;
using Wpf.Ui.Controls;

namespace GammaGear.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ICollection<object> _menuItems = new ObservableCollection<object>()
        {
            new NavigationViewItem("Home", SymbolRegular.Home24, typeof(HomePage))
            {
                Content = "Home",
                FontSize = 22,
                Margin = new Thickness(0, 10, 0, 0)
            },
            new NavigationViewItemSeparator(),
            new NavigationViewItem("Loadouts", SymbolRegular.Briefcase24, typeof(LoadoutsPage))
            {
                FontSize = 22
            },
            new NavigationViewItem("Items", SymbolRegular.HatGraduation24, typeof(ItemsPage))
            {
                FontSize = 22
            },
            new NavigationViewItem("Pets", SymbolRegular.AnimalCat24, typeof(PetsPage))
            {
                FontSize = 22
            },
        };

        public ICollection<object> MenuItems => _menuItems;

        private readonly ICollection<object> _footerItems = new ObservableCollection<object>()
        {
            new NavigationViewItem("Settings", SymbolRegular.Settings24, typeof(SettingsPage))
            {
                FontSize = 22
            },
            new NavigationViewItem("Debug", SymbolRegular.Bug24, typeof(DebugPage))
            {
                FontSize = 22,
                Visibility = System.Windows.Visibility.Collapsed
            },
            new NavigationViewItem("About", SymbolRegular.Info24, typeof(AboutPage))
            {
                FontSize = 22
            }
        };

        public ICollection<object> FooterItems => _footerItems;
    }
}
