using CommunityToolkit.Mvvm.Input;
using GammaGear.Extensions;
using GammaGear.ViewModels;
using GammaGear.ViewModels.Pages;
using GammaItems;
using Microsoft.Extensions.Logging;
using SQLitePCL;
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
using FilterButtonInfo = GammaGear.ViewModels.FilterButtonInfo;
using FilterSchoolInfo = GammaGear.ViewModels.FilterSchoolInfo;
using FilterEquipmentInfo = GammaGear.ViewModels.FilterEquipmentInfo;
using GammaGear.ViewModels.Controls;

namespace GammaGear.Views.Pages
{
    /// <summary>
    /// Interaction logic for Items.xaml
    /// </summary>
    public partial class ItemsPage : Page
    {
        public ItemsPageViewModel ViewModel { get; }
        public int FilterMinLevel { get; set; } = 1;
        public int FilterMaxLevel { get; set; } = 170;
        public School FilterSchool { get; set; } = School.Universal;
        public ItemType FilterItemType { get; set; } = ItemType.None;
        public bool FilterNoAuction { get; set; } = true;
        public bool FilterNoTrade { get; set; } = true;
        public bool FilterCrownsOnly { get; set; } = true;
        public bool FilterPVPOnly { get; set; } = false;
        public bool FilterRetired { get; set; } = false;
        public bool FilterDebug { get; set; } = false;
        public List<FilterButtonInfo> FilterOptionButtons { get; set; } = new List<FilterButtonInfo>()
        {
            new FilterButtonInfo("Assets/Images/(Icon)_Stats_Health.png", "Health", FilterOption.Health),
            new FilterButtonInfo("Assets/Images/(Icon)_Stats_Mana.png", "Mana", FilterOption.Mana),
            new FilterButtonInfo("Assets/Images/(Icon)_Stats_Energy.png", "Energy", FilterOption.Energy),
            new FilterButtonInfo("Assets/Images/(Icon)_Stats_Damage.png", "Damage", FilterOption.Damage),
            new FilterButtonInfo("Assets/Images/(Icon)_Stats_Resistance.png", "Resistance", FilterOption.Resistance),
            new FilterButtonInfo("Assets/Images/(Icon)_Stats_Accuracy.png", "Accuracy", FilterOption.Accuracy),
            new FilterButtonInfo("Assets/Images/(Icon)_Stats_Power_Pip.png", "Powerpip Chance", FilterOption.PowerPipChance),
            new FilterButtonInfo("Assets/Images/(Icon)_Stats_Critical.png", "Critical Rating", FilterOption.CriticalRating),
            new FilterButtonInfo("Assets/Images/(Icon)_Stats_Critical_Block.png", "Critical Block Rating", FilterOption.CriticalBlock),
            new FilterButtonInfo("Assets/Images/(Icon)_Stats_Armor_Piercing.png", "Armor Piercing", FilterOption.ArmorPiercing),
            new FilterButtonInfo("Assets/Images/(Icon)_Stats_Pip_Conversion.png", "Pip Conversion", FilterOption.PipConversion),
            new FilterButtonInfo("Assets/Images/(Icon)_Stats_Shadow_Pip.png", "Shadow Pip Rating", FilterOption.ShadowPipRating),
            new FilterButtonInfo("Assets/Images/(Icon)_Stats_Stun_Resistance.png", "Stun Resistance", FilterOption.StunResistance),
            new FilterButtonInfo("Assets/Images/(Icon)_Stats_SpeedBonus.png", "Speed Bonus", FilterOption.MovementSpeed),
            new FilterButtonInfo("Assets/Images/(Icon)_Stats_Fishing_Luck.png", "Fishing Luck", FilterOption.FishingLuck),
            new FilterButtonInfo("Assets/Images/(Icon)_Stats_Archmastery.png", "Archmastery Rating", FilterOption.ArchmasteryRating),
            new FilterButtonInfo("Assets/Images/(Icon)_Stats_Flat_Damage.png", "Flat Damage", FilterOption.FlatDamage),
            new FilterButtonInfo("Assets/Images/(Icon)_Stats_Flat_Resistance.png", "Flat Resistance", FilterOption.FlatResistance),
            new FilterButtonInfo("Assets/Images/(Icon)_Stats_Incoming.png", "Incoming Healing", FilterOption.Incoming),
            new FilterButtonInfo("Assets/Images/(Icon)_Stats_Outgoing.png", "Outgoing Healing", FilterOption.Outgoing),
            new FilterButtonInfo("Assets/Images/(Icon)_Stats_Pip.png", "Extra Pips", FilterOption.ExtraPips),
            new FilterButtonInfo("Assets/Images/(Icon)_Stats_Power_Pip.png", "Extra Power Pips", FilterOption.ExtraPowerPips),
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
        public ObservableCollection<FilterData> ActiveFilters { get; set; } = new ObservableCollection<FilterData>();

        private ILogger<ItemsPage> _logger;

        public ItemsPage(
            ItemsPageViewModel viewModel,
            ILogger<ItemsPage> logger)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();

            _logger = logger;

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
                   (FilterSchool == School.Universal || FilterSchool == item.SchoolRequirement || (item.SchoolRequirement == School.Universal && item.SchoolRestriction != FilterSchool)) &&
                   (FilterItemType == ItemType.None || FilterItemType == item.Type) &&
                   (FilterMinLevel <= item.LevelRequirement && FilterMaxLevel >= item.LevelRequirement) &&
                   (FilterNoAuction || !item.Flags.HasFlag(ItemFlags.FLAG_NoAuction)) &&
                   (FilterNoTrade || !item.Flags.HasFlag(ItemFlags.FLAG_NoTrade)) &&
                   (FilterCrownsOnly || !item.Flags.HasFlag(ItemFlags.FLAG_CrownsOnly)) &&
                   (FilterPVPOnly || !item.Flags.HasFlag(ItemFlags.FLAG_PVPOnly)) &&
                   (FilterRetired || !item.Flags.HasFlag(ItemFlags.FLAG_Retired)) &&
                   (FilterDebug || !item.Flags.HasFlag(ItemFlags.FLAG_DevItem));
        }

        private void OnSortButtonClick(object sender, RoutedEventArgs e)
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

        private void OnFilterFlyoutButtonClick(object sender, RoutedEventArgs e)
        {
            FilterFlyout.IsOpen = true;
        }

        private IRelayCommand _onFilterButtonClicked;
        public IRelayCommand OnFilterButtonClicked => _onFilterButtonClicked ??= new RelayCommand(FilterButtonClicked);
        private void FilterButtonClicked()
        {
            _logger.LogInformation("Filter button clicked...");
            _logger.LogInformation("Min Level is: {FilterMinLevel}", FilterMinLevel);
            _logger.LogInformation("Max Level is: {FilterMaxLevel}", FilterMaxLevel);
            WizItemsView.Items.Filter = ItemsFilter;
            FilterFlyout.IsOpen = false;
        }

        private IRelayCommand<FilterOption?> _onFilterAdded;
        public IRelayCommand<FilterOption?> OnFilterAdded => _onFilterAdded ??= new RelayCommand<FilterOption?>(FilterAddButtonClicked);
        private void FilterAddButtonClicked(FilterOption? filterOption)
        {
            if (filterOption != null)
            {
                if (!ActiveFilters.Any(item => item.FilterOption == filterOption))
                {
                    // Filter doesn't exist.
                    ActiveFilters.Add(new FilterData((FilterOption)filterOption, School.Fire));
                }
            }
        }

        //private IRelayCommand<FilterOption?> _onFilterAdded;
        //public IRelayCommand<FilterOption?> OnFilterAdded => _onFilterAdded ??= new RelayCommand<FilterOption?>(FilterAddButtonClicked);
        //private void FilterAddButtonClicked(FilterOption? filterOption)
        //{
        //    if (filterOption != null)
        //    {
        //        if (!ActiveFilters.Any(item => item.FilterOption == filterOption))
        //        {
        //            // Filter doesn't exist.
        //            ActiveFilters.Add(new FilterData((FilterOption)filterOption, School.Fire));
        //        }
        //    }
        //}
    }
}
