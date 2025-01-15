using GammaGear.Extensions;
using GammaGear.ViewModels;
using GammaGear.ViewModels.Pages;
using GammaItems;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
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
using Button = Wpf.Ui.Controls.Button;
using Image = Wpf.Ui.Controls.Image;

namespace GammaGear.Views.Pages
{
    /// <summary>
    /// Interaction logic for Items.xaml
    /// </summary>
    public partial class ItemsPage : Page
    {
        public class FilterButtonInfo
        {
            public Uri ImageSource { get; set; }
            public string Tooltip { get; set; }

            public FilterButtonInfo(string path, string tooltip)
            {
                ImageSource = new Uri(@"pack://application:,,,/GammaGear;component/" + path);
                Tooltip = tooltip;
            }
        }
        public class FilterSchoolInfo
        {
            public Uri ImageSource { get; set; }
            public School Name { get; set; }

            public FilterSchoolInfo(School name)
            {
                ImageSource = name.ToIconUri();
                Name = name;
            }
        }
        public class FilterEquipmentInfo
        {
            public Uri ImageSource { get; set; }
            public ItemType Name { get; set; }

            public FilterEquipmentInfo(ItemType name)
            {
                ImageSource = name.ToIconUri();
                Name = name;
            }
        }
        public ItemsPageViewModel ViewModel { get; }
        public School FilterSchool { get; set; } = School.None;
        public ItemType FilterItemType { get; set; } = ItemType.None;
        public List<FilterButtonInfo> FilterOptionButtons { get; set; } = new List<FilterButtonInfo>()
        {
            new FilterButtonInfo("Assets/Images/(Icon)_Stats_Health.png", "Health"),
            new FilterButtonInfo("Assets/Images/(Icon)_Stats_Mana.png", "Mana"),
            new FilterButtonInfo("Assets/Images/(Icon)_Stats_Energy.png", "Energy"),
            new FilterButtonInfo("Assets/Images/(Icon)_Stats_Damage.png", "Damage"),
            new FilterButtonInfo("Assets/Images/(Icon)_Stats_Resistance.png", "Resistance"),
            new FilterButtonInfo("Assets/Images/(Icon)_Stats_Accuracy.png", "Accuracy"),
            new FilterButtonInfo("Assets/Images/(Icon)_Stats_Power_Pip.png", "Powerpip Chance"),
            new FilterButtonInfo("Assets/Images/(Icon)_Stats_Critical.png", "Critical Rating"),
            new FilterButtonInfo("Assets/Images/(Icon)_Stats_Critical_Block.png", "Critical Block Rating"),
            new FilterButtonInfo("Assets/Images/(Icon)_Stats_Armor_Piercing.png", "Armor Piercing"),
            new FilterButtonInfo("Assets/Images/(Icon)_Stats_Pip_Conversion.png", "Pip Conversion"),
            new FilterButtonInfo("Assets/Images/(Icon)_Stats_Shadow_Pip.png", "Shadow Pip Rating"),
            new FilterButtonInfo("Assets/Images/(Icon)_Stats_Stun_Resistance.png", "Stun Resistance"),
            new FilterButtonInfo("Assets/Images/(Icon)_Stats_SpeedBonus.png", "Speed Bonus"),
            new FilterButtonInfo("Assets/Images/(Icon)_Stats_Fishing_Luck.png", "Fishing Luck"),
            new FilterButtonInfo("Assets/Images/(Icon)_Stats_Archmastery.png", "Archmastery Rating"),
            new FilterButtonInfo("Assets/Images/(Icon)_Stats_Flat_Damage.png", "Flat Damage"),
            new FilterButtonInfo("Assets/Images/(Icon)_Stats_Flat_Resistance.png", "Flat Resistance"),
            new FilterButtonInfo("Assets/Images/(Icon)_Stats_Incoming.png", "Incoming Healing"),
            new FilterButtonInfo("Assets/Images/(Icon)_Stats_Outgoing.png", "Outgoing Healing"),
            new FilterButtonInfo("Assets/Images/(Icon)_Stats_Pip.png", "Extra Pips"),
            new FilterButtonInfo("Assets/Images/(Icon)_Stats_Power_Pip.png", "Extra Power Pips"),
        };
        public List<FilterSchoolInfo> FilterSchools { get; set; } = new List<FilterSchoolInfo>()
        {
            new FilterSchoolInfo(School.Universal),
            new FilterSchoolInfo(School.Fire),
            new FilterSchoolInfo(School.Ice),
            new FilterSchoolInfo(School.Storm),
            new FilterSchoolInfo(School.Myth),
            new FilterSchoolInfo(School.Life),
            new FilterSchoolInfo(School.Death),
            new FilterSchoolInfo(School.Balance),
        };
        public List<FilterEquipmentInfo> FilterItemTypes { get; set; } = new List<FilterEquipmentInfo>()
        {
            new FilterEquipmentInfo(ItemType.None),
            new FilterEquipmentInfo(ItemType.Hat),
            new FilterEquipmentInfo(ItemType.Robe),
            new FilterEquipmentInfo(ItemType.Boots),
            new FilterEquipmentInfo(ItemType.Wand),
            new FilterEquipmentInfo(ItemType.Athame),
            new FilterEquipmentInfo(ItemType.Amulet),
            new FilterEquipmentInfo(ItemType.Ring),
            new FilterEquipmentInfo(ItemType.Deck),
            new FilterEquipmentInfo(ItemType.Pet),
            new FilterEquipmentInfo(ItemType.Mount),
            new FilterEquipmentInfo(ItemType.TearJewel),
            new FilterEquipmentInfo(ItemType.CircleJewel),
            new FilterEquipmentInfo(ItemType.SquareJewel),
            new FilterEquipmentInfo(ItemType.TriangleJewel),
            new FilterEquipmentInfo(ItemType.PinSquarePip),
            new FilterEquipmentInfo(ItemType.PinSquareShield),
            new FilterEquipmentInfo(ItemType.PinSquareSword),
        };

        public ItemsPage(
            ItemsPageViewModel viewModel,
            INavigationService navigationService)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();

            WizItemsView.Items.Filter = ItemsFilter;

            //navigationService.GetNavigationControl().Navigating += OnNavigate;
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

        private void FilterFlyoutButton_Click(object sender, RoutedEventArgs e)
        {
            FilterFlyout.IsOpen = true;
        }
    }
}
