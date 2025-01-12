using GammaGear.ViewModels;
using GammaGear.ViewModels.Pages;
using GammaItems;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace GammaGear.Views.Pages
{
    /// <summary>
    /// Interaction logic for Items.xaml
    /// </summary>
    public partial class ItemsPage : Page
    {
        public ItemsPageViewModel ViewModel { get; }
        public School FilterSchool { get; set; } = School.None;
        public ItemType FilterItemType { get; set; } = ItemType.None;

        public ItemsPage(
            ItemsPageViewModel viewModel,
            INavigationService navigationService)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();

            WizItemsView.Items.Filter = ItemsFilter;

            navigationService.GetNavigationControl().Navigating += OnNavigate;
        }

        private void OnNavigate(NavigationView sender, Wpf.Ui.Controls.NavigatingCancelEventArgs args)
        {
            if (args.Page.Equals(this))
            {
                // Navigating to this page
                FilterToggle.IsChecked = false;
            }
        }

        private bool ItemsFilter(object obj)
        {
            if (obj is not ItemViewModel item)
            {
                return false;
            }

            return !item.IsDebug &&
                   (FilterSchool == School.None || FilterSchool == item.SchoolRequirement) &&
                   (FilterItemType == ItemType.None || FilterItemType == item.Type);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WizItemsView.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                if (e.AddedItems[0] is ItemViewModel item)
                {
                    ViewModel.SelectedItemChanged(item);
                }
            }
        }
    }
}
