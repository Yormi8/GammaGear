using GammaGear.ViewModels;
using GammaGear.ViewModels.Pages;
using GammaItems;
using System;
using System.Collections.Generic;
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
            ItemsPageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();

            WizItemsView.Items.Filter = ItemsFilter;
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
    }
}
