using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GammaGear.Properties;
using GammaGear.Source;
using Microsoft.Win32;
using W101ToolUI.Source;
using Microsoft.WindowsAPICodePack.Dialogs;
using static GammaGear.Source.Item;

namespace GammaGear
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ItemFilter DBItemFilters = new ItemFilter();
        private ItemLoadout mainLoadout = new ItemLoadout();
        private bool showDBItemIDs = false;
        public MainWindow()
        {
            InitializeComponent();

            ICollectionView cvItems = CollectionViewSource.GetDefaultView(ItemDatabaseGrid.ItemsSource);
            if (cvItems != null && cvItems.CanSort == true)
            {
                cvItems.SortDescriptions.Clear();
                cvItems.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                cvItems.SortDescriptions.Add(new SortDescription("DisplayType", ListSortDirection.Ascending));
                cvItems.SortDescriptions.Add(new SortDescription("DisplaySchool", ListSortDirection.Ascending));
                cvItems.SortDescriptions.Add(new SortDescription("LevelRequirement", ListSortDirection.Ascending));
            }

            DamageTextBoxes = new TextBox[]
            {
                StatsFireDamage,
                StatsIceDamage,
                StatsStormDamage,
                StatsMythDamage,
                StatsLifeDamage,
                StatsDeathDamage,
                StatsBalanceDamage,
                StatsShadowDamage,
            };
            FlatDamageTextBoxes = new TextBox[]
            {
                StatsFireFlatDamage,
                StatsIceFlatDamage,
                StatsStormFlatDamage,
                StatsMythFlatDamage,
                StatsLifeFlatDamage,
                StatsDeathFlatDamage,
                StatsBalanceFlatDamage,
                StatsShadowFlatDamage,
            };
            ResistTextBoxes = new TextBox[]
            {
                StatsFireResistance,
                StatsIceResistance,
                StatsStormResistance,
                StatsMythResistance,
                StatsLifeResistance,
                StatsDeathResistance,
                StatsBalanceResistance,
                StatsShadowResistance,
            };
            FlatResistanceTextBoxes = new TextBox[]
            {
                StatsFireFlatResistance,
                StatsIceFlatResistance,
                StatsStormFlatResistance,
                StatsMythFlatResistance,
                StatsLifeFlatResistance,
                StatsDeathFlatResistance,
                StatsBalanceFlatResistance,
                StatsShadowFlatResistance,
            };
            AccuracyTextBoxes = new TextBox[]
            {
                StatsFireAccuracy,
                StatsIceAccuracy,
                StatsStormAccuracy,
                StatsMythAccuracy,
                StatsLifeAccuracy,
                StatsDeathAccuracy,
                StatsBalanceAccuracy,
                StatsShadowAccuracy,
            };
            CriticalTextBoxes = new TextBox[]
            {
                StatsFireCritical,
                StatsIceCritical,
                StatsStormCritical,
                StatsMythCritical,
                StatsLifeCritical,
                StatsDeathCritical,
                StatsBalanceCritical,
                StatsShadowCritical,
            };
            CriticalBlockTextBoxes = new TextBox[]
            {
                StatsFireCriticalBlock,
                StatsIceCriticalBlock,
                StatsStormCriticalBlock,
                StatsMythCriticalBlock,
                StatsLifeCriticalBlock,
                StatsDeathCriticalBlock,
                StatsBalanceCriticalBlock,
                StatsShadowCriticalBlock,
            };
            PierceTextBoxes = new TextBox[]
            {
                StatsFirePierce,
                StatsIcePierce,
                StatsStormPierce,
                StatsMythPierce,
                StatsLifePierce,
                StatsDeathPierce,
                StatsBalancePierce,
                StatsShadowPierce,
            }; 
            PipConversionTextBoxes = new TextBox[]
            {
                StatsFirePipConversion,
                StatsIcePipConversion,
                StatsStormPipConversion,
                StatsMythPipConversion,
                StatsLifePipConversion,
                StatsDeathPipConversion,
                StatsBalancePipConversion,
                StatsShadowPipConversion,
            };

            UpdateStatsTab();
        }
        private void OnWikiMenuItemClicked(object sender, ExecutedRoutedEventArgs e)
        {
            ProcessStartInfo wiki = new ProcessStartInfo
            {
                FileName = "https://www.wizard101central.com/wiki/Wizard101_Wiki",
                UseShellExecute = true
            };
            Process.Start(wiki);
        }
        private void OnSelectDatabaseTabItem(object sender, RoutedEventArgs e)
        {
            DatabaseTabItem.IsSelected = true;
            if (sender is Button button)
            {
                switch (button.Name)
                {
                    case "WizardHatBox":
                        DBTypeBox.SelectedIndex = (int)Item.ItemType.Hat + 1;
                        break;
                    case "WizardRobeBox":
                        DBTypeBox.SelectedIndex = (int)Item.ItemType.Robe + 1;
                        break;
                    case "WizardBootsBox":
                        DBTypeBox.SelectedIndex = (int)Item.ItemType.Boots + 1;
                        break;
                    case "WizardWandBox":
                        DBTypeBox.SelectedIndex = (int)Item.ItemType.Wand + 1;
                        break;
                    case "WizardAthameBox":
                        DBTypeBox.SelectedIndex = (int)Item.ItemType.Athame + 1;
                        break;
                    case "WizardAmuletBox":
                        DBTypeBox.SelectedIndex = (int)Item.ItemType.Amulet + 1;
                        break;
                    case "WizardRingBox":
                        DBTypeBox.SelectedIndex = (int)Item.ItemType.Ring + 1;
                        break;
                    case "WizardPetBox":
                        DBTypeBox.SelectedIndex = (int)Item.ItemType.Pet + 1;
                        break;
                    case "WizardDeckBox":
                        DBTypeBox.SelectedIndex = (int)Item.ItemType.Deck + 1;
                        break;
                    case "WizardMountBox":
                        DBTypeBox.SelectedIndex = (int)Item.ItemType.Mount + 1;
                        break;
                    default:
                        break;
                }
            }
            DBSearchButtonOnClick(null, null);
        }

        private void DBResetButtonOnClick(object sender, RoutedEventArgs eventArgs)
        {
            DBNameBox.Text = "";
            DBTypeBox.SelectedIndex = 0;
            DBSchoolBox.SelectedIndex = 0;
            DBMinLevelBox.Value = 1;
            DBMaxLevelBox.Value = 160;

            DBSearchButtonOnClick(sender, eventArgs);
        }
        private void DBSearchButtonOnClick(object sender, RoutedEventArgs eventArgs)
        {
            DBItemFilters.Name = DBNameBox.Text;
            DBItemFilters.Type = DBTypeBox.SelectedIndex - 1 == -1 ? Item.ItemType.None : (Item.ItemType)DBTypeBox.SelectedIndex - 1;
            DBItemFilters.School = DBSchoolBox.SelectedIndex - 1 == -1 ? Item.School.None : (Item.School)DBSchoolBox.SelectedIndex - 1; ;
            DBItemFilters.MinLevel = DBMinLevelBox.Value ?? 1;
            DBItemFilters.MaxLevel = DBMaxLevelBox.Value ?? 160;

            ICollectionView cvItems = CollectionViewSource.GetDefaultView(ItemDatabaseGrid.ItemsSource);
            cvItems.Refresh();
        }
        private void SetItemContent(ItemDisplay item, StackPanel parent)
        {
            parent.Children.Clear();
            TextBlock tb = item.GetStatDisplay(showDBItemIDs);
            parent.Children.Add(tb);
        }
        private void DBItemSelected(object sender, RoutedEventArgs eventArgs)
        {
            if (!IsVisible) return;
            if ((sender as DataGrid).SelectedItem is not ItemDisplay item) return;
            //MessageBox.Show(item.Name);

            DBSelectedItemName.Text = item.Name;
            SetItemContent(item, DBSelectedItemContent);

            if (mainLoadout.GetNumberAllowedEquipped(item.Type) == 1 && mainLoadout.GetNumberOfEquipped(item.Type) == 1)
            {
                ItemDisplay equipped = mainLoadout.GetEquippedFromType(item.Type);
                if (equipped.ID == item.ID)
                {
                    DBEquipButtonImage.Visibility = Visibility.Hidden;
                    DBUnequipButtonImage.Visibility = Visibility.Visible;
                }
                else
                {
                    DBEquipButtonImage.Visibility = Visibility.Visible;
                    DBUnequipButtonImage.Visibility = Visibility.Hidden;
                }
                DBEquippedItemName.Text = equipped.Name;
                SetItemContent(mainLoadout.GetEquippedFromType(item.Type), DBEquippedItemContent);
            }
            else
            {
                DBEquipButtonImage.Visibility = Visibility.Visible;
                DBUnequipButtonImage.Visibility = Visibility.Hidden;
                DBEquippedItemName.Text = "None";
                DBEquippedItemContent.Children.Clear();
            }
        }
        private void DBItemUnselected(object sender, RoutedEventArgs eventArgs)
        {
            
        }
        private void CollectionViewSource_Filter(object sender, FilterEventArgs eventArgs)
        {
            if (eventArgs.Item is ItemDisplay item)
            {
                eventArgs.Accepted = true;
                if (!string.IsNullOrEmpty(DBItemFilters.Name) && !item.Name.ToLower().Contains(DBItemFilters.Name.ToLower()))
                {
                    eventArgs.Accepted = false;
                }
                if (DBItemFilters.Type != Item.ItemType.None && DBItemFilters.Type != item.Type)
                {
                    eventArgs.Accepted = false;
                }
                if (DBItemFilters.School != Item.School.None && DBItemFilters.School != item.SchoolRequirement)
                {
                    eventArgs.Accepted = false;
                }
                if (DBItemFilters.MinLevel > item.LevelRequirement || DBItemFilters.MaxLevel < item.LevelRequirement)
                {
                    eventArgs.Accepted = false;
                }
            }
        }

        readonly TextBox[] DamageTextBoxes;
        readonly TextBox[] FlatDamageTextBoxes;
        readonly TextBox[] ResistTextBoxes;
        readonly TextBox[] FlatResistanceTextBoxes;
        readonly TextBox[] AccuracyTextBoxes;
        readonly TextBox[] CriticalTextBoxes;
        readonly TextBox[] CriticalBlockTextBoxes;
        readonly TextBox[] PierceTextBoxes;
        readonly TextBox[] PipConversionTextBoxes;
        private void UpdateStatsTab()
        {
            const string fw = "#,0";
            const string fp = "0\\%";

            mainLoadout.WizardLevel = (int)WizardLevelBox.Value;
            mainLoadout.WizardSchool = (Item.School)WizardSchoolBox.SelectedIndex;

            ItemDisplay Output = mainLoadout.CalculateStats();

            BasicStatsMaxHealth.Text = Output.MaxHealth.ToString(fw);
            BasicStatsMaxMana.Text = Output.MaxMana.ToString(fw);
            BasicStatsMaxEnergy.Text = Output.MaxEnergy.ToString(fw);
            BasicStatsPipChance.Text = Output.PowerpipChance.ToString(fp);
            BasicStatsShadowRating.Text = Output.ShadowpipRating.ToString(fw);
            BasicStatsArchmastery.Text = Output.ArchmasteryRating.ToString(fw);
            BasicStatsStunResist.Text = Output.StunResistChance.ToString(fp);
            BasicStatsIncHealing.Text = Output.IncomingHealing.ToString(fp);
            BasicStatsOutHealing.Text = Output.OutgoingHealing.ToString(fp);
            BasicStatsFishingLuck.Text = Output.FishingLuck.ToString(fp);

            // Add the school damage and universal damage to the output textboxes.
            for (int i = 0; i < 8; i++)
            {
                DamageTextBoxes[i].Text =           (Output.Damages.GetValueOrDefault((Item.School)i + 1) +         Output.Damages.GetValueOrDefault(Item.School.Universal)).ToString(fp);
                FlatDamageTextBoxes[i].Text =       (Output.FlatDamages.GetValueOrDefault((Item.School)i + 1) +     Output.FlatDamages.GetValueOrDefault(Item.School.Universal)).ToString(fw);
                ResistTextBoxes[i].Text =           (Output.Resists.GetValueOrDefault((Item.School)i + 1) +         Output.Resists.GetValueOrDefault(Item.School.Universal)).ToString(fp);
                FlatResistanceTextBoxes[i].Text =   (Output.FlatResists.GetValueOrDefault((Item.School)i + 1) +     Output.FlatResists.GetValueOrDefault(Item.School.Universal)).ToString(fw);
                AccuracyTextBoxes[i].Text =         (Output.Accuracies.GetValueOrDefault((Item.School)i + 1) +      Output.Accuracies.GetValueOrDefault(Item.School.Universal)).ToString(fp);
                CriticalTextBoxes[i].Text =         (Output.Criticals.GetValueOrDefault((Item.School)i + 1) +       Output.Criticals.GetValueOrDefault(Item.School.Universal)).ToString(fw);
                CriticalBlockTextBoxes[i].Text =    (Output.Blocks.GetValueOrDefault((Item.School)i + 1) +          Output.Blocks.GetValueOrDefault(Item.School.Universal)).ToString(fw);
                PierceTextBoxes[i].Text =           (Output.Pierces.GetValueOrDefault((Item.School)i + 1) +         Output.Pierces.GetValueOrDefault(Item.School.Universal)).ToString(fp);
                PipConversionTextBoxes[i].Text =    (Output.PipConversions.GetValueOrDefault((Item.School)i + 1) +  Output.PipConversions.GetValueOrDefault(Item.School.Universal)).ToString(fw);
            }

            // Update the button's text
            WizardHatBox.Content    = mainLoadout.GetEquippedFromType(Item.ItemType.Hat)?.Name ?? "None";
            WizardRobeBox.Content   = mainLoadout.GetEquippedFromType(Item.ItemType.Robe)?.Name ?? "None";
            WizardBootsBox.Content  = mainLoadout.GetEquippedFromType(Item.ItemType.Boots)?.Name ?? "None";
            WizardWandBox.Content   = mainLoadout.GetEquippedFromType(Item.ItemType.Wand)?.Name ?? "None";
            WizardAthameBox.Content = mainLoadout.GetEquippedFromType(Item.ItemType.Athame)?.Name ?? "None";
            WizardAmuletBox.Content = mainLoadout.GetEquippedFromType(Item.ItemType.Amulet)?.Name ?? "None";
            WizardRingBox.Content   = mainLoadout.GetEquippedFromType(Item.ItemType.Ring)?.Name ?? "None";
            WizardPetBox.Content    = mainLoadout.GetEquippedFromType(Item.ItemType.Pet)?.Name ?? "None";
            WizardDeckBox.Content   = mainLoadout.GetEquippedFromType(Item.ItemType.Deck)?.Name ?? "None";
            WizardMountBox.Content  = mainLoadout.GetEquippedFromType(Item.ItemType.Mount)?.Name ?? "None";
        }
        private void TabControlSelectionChanged(object sender, SelectionChangedEventArgs eventArgs)
        {
            if (!this.IsVisible) return;

            string tabItem = ((sender as TabControl)?.SelectedItem as TabItem)?.Name;

            switch (tabItem)
            {
                case "StatsTab":
                    UpdateStatsTab();
                    break;
                default:
                    return;
            }
        }
        private void DBEquipButton(object sender, RoutedEventArgs eventArgs)
        {
            if (ItemDatabaseGrid.SelectedItem is ItemDisplay selectedItem)
            {
                ItemDisplay equipped = mainLoadout.GetEquippedFromType(selectedItem.Type);
                if (equipped?.ID == selectedItem?.ID)
                {
                    mainLoadout.UnequipItem(selectedItem.Type);
                }
                else
                {
                    mainLoadout.EquipItem(selectedItem);
                }

                DBItemSelected(ItemDatabaseGrid, null);
            }

        }
        private void DBTrashButton(object sender, RoutedEventArgs eventArgs)
        {
            if (ItemDatabaseGrid.SelectedItem is ItemDisplay selectedItem)
            {

            }
        }
        private void DBWikiButton(object sender, RoutedEventArgs eventArgs)
        {
            if (ItemDatabaseGrid.SelectedItem is ItemDisplay selectedItem)
            {
                ProcessStartInfo wiki = new ProcessStartInfo
                {
                    FileName = "https://www.wizard101central.com/wiki/Item:" + selectedItem.Name.Replace(' ', '_'),
                    UseShellExecute = true
                };
                Process.Start(wiki);
            }
        }
        private void UpdateStatsTabEvent(object sender, RoutedEventArgs eventArgs)
        {
            if (this.IsVisible)
            {
                UpdateStatsTab();
            }
        }
        private void FileCreateNewDB(object sender, RoutedEventArgs eventArgs)
        {
            if (string.IsNullOrEmpty(Settings.Default.LocaleFolder))
            {
                CommonOpenFileDialog chooseLocaleDialog = new CommonOpenFileDialog()
                {
                    IsFolderPicker = true,
                };
                if (chooseLocaleDialog.ShowDialog() != CommonFileDialogResult.Ok) return;
                Settings.Default.LocaleFolder = chooseLocaleDialog.FileName;
            }
            OpenFileDialog ofd = new OpenFileDialog
            {
                
                DefaultExt = "txt",
                Filter = "Text Files (*.txt)|*.txt|All files (*.*)|*.*",
                InitialDirectory = Environment.CurrentDirectory,
            };                 
            if (ofd.ShowDialog() == false)
            {
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog
            {
                DefaultExt = "sqlite",
                Filter = "SQLite Database Files (*.sqlite)|*.sqlite|All files (*.*)|*.*",
                InitialDirectory = Environment.CurrentDirectory,
            };
            if (sfd.ShowDialog() == false)
            {
                return;
            }

            XmlToDb xdb = new XmlToDb();
            xdb.CreateDbFromList(ofd.FileName, sfd.FileName);
        }

        private void FileLoadDB(object sender, RoutedEventArgs eventArgs)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Title = "Open Database",
                DefaultExt = "txt",
                Filter = "SQLite Database Files (*.sqlite)|*.sqlite|All files (*.*)|*.*",
                InitialDirectory = Environment.CurrentDirectory,
            };
            if (ofd.ShowDialog() == false)
            {
                return;
            }

            XmlToDb xdb = new XmlToDb();
            ItemViewModel items = this.Resources["itemVM"] as ItemViewModel;
            var newItems = xdb.CreateListFromDB(ofd.FileName);

            foreach (var item in newItems.Item1.Where(i => i.Type >= ItemType.Hat && i.Type <= ItemType.PinSquareSword))
            {
                items.Add((ItemDisplay)item);
            }
        }
    }

    public static class W101Commands
    {
        public static readonly RoutedUICommand OpenWiki = new RoutedUICommand
        (
            "Opens the W101 Wiki in the user's browser.",
            "OpenWiki",
            typeof(W101Commands)
        );
    }

    public class ItemViewModel : ObservableCollection<ItemDisplay>
    {
        public ItemViewModel() { }
    }

    public class ItemDisplay : INotifyPropertyChanged
    {
        private Item backingItem;
        public string Name { get => backingItem.Name; set => backingItem.Name = value; }
        public Guid ID { get => backingItem.ID; set => backingItem.ID = value; }
        public Guid KI_ID { get => backingItem.KI_ID; set => backingItem.KI_ID = value; }
        public Guid KI_SetBonusID { get => backingItem.KI_SetBonusID; set => backingItem.KI_SetBonusID = value; }
        public ItemType Type { get => backingItem.Type; set => backingItem.Type = value; }
        public int LevelRequirement { get => backingItem.LevelRequirement; set => backingItem.LevelRequirement = value; }
        public ItemFlags Flags { get => backingItem.Flags; set => backingItem.Flags = value; }
        public ArenaRank PVPRankRequirement { get => backingItem.PVPRankRequirement; set => backingItem.PVPRankRequirement = value; }
        public ArenaRank PetRankRequirement { get => backingItem.PetRankRequirement; set => backingItem.PetRankRequirement = value; }
        public School SchoolRequirement { get => backingItem.SchoolRequirement; set => backingItem.SchoolRequirement = value; }
        public School SchoolRestriction { get => backingItem.SchoolRestriction; set => backingItem.SchoolRestriction = value; }
        public bool Retired { get => backingItem.Retired; set => backingItem.Retired = value; }
        public int MaxHealth { get => backingItem.MaxHealth; set => backingItem.MaxHealth = value; }
        public int MaxMana { get => backingItem.MaxMana; set => backingItem.MaxMana = value; }
        public int MaxEnergy { get => backingItem.MaxEnergy; set => backingItem.MaxEnergy = value; }
        public int SpeedBonus { get => backingItem.SpeedBonus; set => backingItem.SpeedBonus = value; }
        public int PowerpipChance { get => backingItem.PowerpipChance; set => backingItem.PowerpipChance = value; }
        public int ShadowpipRating { get => backingItem.ShadowpipRating; set => backingItem.ShadowpipRating = value; }
        public int StunResistChance { get => backingItem.StunResistChance; set => backingItem.StunResistChance = value; }
        public int FishingLuck { get => backingItem.FishingLuck; set => backingItem.FishingLuck = value; }
        public int ArchmasteryRating { get => backingItem.ArchmasteryRating; set => backingItem.ArchmasteryRating = value; }
        public int IncomingHealing { get => backingItem.IncomingHealing; set => backingItem.IncomingHealing = value; }
        public int OutgoingHealing { get => backingItem.OutgoingHealing; set => backingItem.OutgoingHealing = value; }
        public int PipsGiven { get => backingItem.PipsGiven; set => backingItem.PipsGiven = value; }
        public int PowerpipsGiven { get => backingItem.PowerpipsGiven; set => backingItem.PowerpipsGiven = value; }
        public School AltSchoolMastery { get => backingItem.AltSchoolMastery; set => backingItem.AltSchoolMastery = value; }
        public int TearJewelSlots { get => backingItem.TearJewelSlots; set => backingItem.TearJewelSlots = value; }
        public int CircleJewelSlots { get => backingItem.CircleJewelSlots; set => backingItem.CircleJewelSlots = value; }
        public int SquareJewelSlots { get => backingItem.SquareJewelSlots; set => backingItem.SquareJewelSlots = value; }
        public int TriangleJewelSlots { get => backingItem.TriangleJewelSlots; set => backingItem.TriangleJewelSlots = value; }
        public int PowerPinSlots { get => backingItem.PowerPinSlots; set => backingItem.PowerPinSlots = value; }
        public int ShieldPinSlots { get => backingItem.ShieldPinSlots; set => backingItem.ShieldPinSlots = value; }
        public int SwordPinSlots { get => backingItem.SwordPinSlots; set => backingItem.SwordPinSlots = value; }
        public int SetBonusLevel { get => backingItem.SetBonusLevel; set => backingItem.SetBonusLevel = value; }
        public ItemSetBonus SetBonus { get => backingItem.SetBonus; set => backingItem.SetBonus = value; }
        public Dictionary<School, int> Accuracies { get => backingItem.Accuracies; set => backingItem.Accuracies = value; }
        public Dictionary<School, int> Damages { get => backingItem.Damages; set => backingItem.Damages = value; }
        public Dictionary<School, int> Resists { get => backingItem.Resists; set => backingItem.Resists = value; }
        public Dictionary<School, int> Criticals { get => backingItem.Criticals; set => backingItem.Criticals = value; }
        public Dictionary<School, int> Blocks { get => backingItem.Blocks; set => backingItem.Blocks = value; }
        public Dictionary<School, int> Pierces { get => backingItem.Pierces; set => backingItem.Pierces = value; }
        public Dictionary<School, int> FlatDamages { get => backingItem.FlatDamages; set => backingItem.FlatDamages = value; }
        public Dictionary<School, int> FlatResists { get => backingItem.FlatResists; set => backingItem.FlatResists = value; }
        public Dictionary<School, int> PipConversions { get => backingItem.PipConversions; set => backingItem.PipConversions = value; }
        public Dictionary<string, int> ItemCards { get => backingItem.ItemCards; set => backingItem.ItemCards = value; }
        public string DisplayType => Type switch
        {
            ItemType.Shoes => "Boots",
            ItemType.Weapon => "Wand",
            ItemType.TearJewel => "Tear Jewel",
            ItemType.CircleJewel => "Circle Jewel",
            ItemType.SquareJewel => "Square Jewel",
            ItemType.TriangleJewel => "Triangle Jewel",
            ItemType.PinSquarePower => "Power Pin",
            ItemType.PinSquareShield => "Shield Pin",
            ItemType.PinSquareSword => "Sword Pin",
            _ => Type.ToString()
        };
        public string DisplayTypeSource => Type switch
        {
            ItemType.Shoes => "Assets/Images/(Icon)_Equipment_Boots.png",
            ItemType.Weapon => "Assets/Images/(Icon)_Equipment_Wand.png",
            ItemType.PinSquarePower => "Assets/Images/(Icon)_Equipment_PinSquarePower.png",
            ItemType.None => "Assets/Images/(Icon)_Help.png",
            ItemType.ItemSetBonusData => "Assets/Images/(Icon)_Help.png",
            _ => "Assets/Images/(Icon)_Equipment_" + Type.ToString() + ".png"
        };

        public string DisplaySchool => SchoolRequirement switch
        {
            School.Universal => SchoolRestriction != School.Universal ? 
                "Not " + SchoolRestriction.ToString() : 
                "Any",
            School.None => "Assets/Images/(Icon)_Help.png",
            _ => SchoolRequirement.ToString()
        };

        public string DisplaySchoolSource => SchoolRequirement switch
        {
            School.Universal => SchoolRestriction != School.Universal ?
                "Assets/Images/(Icon)_School_Not_" + SchoolRestriction.ToString() + ".png" :
                "Assets/Images/(Icon)_School_Global.png",
            School.None => "Assets/Images/(Icon)_Help.png",
            _ => "Assets/Images/(Icon)_School_" + SchoolRequirement.ToString() + ".png"
        };
        public bool IsCrownsOnly => Flags.HasFlag(ItemFlags.FLAG_CrownsOnly);
        public Visibility IsCrownsOnlyVisible => IsCrownsOnly ? Visibility.Visible : Visibility.Hidden;
        public bool IsNoAuction => Flags.HasFlag(ItemFlags.FLAG_NoAuction);
        public Visibility IsNoAuctionOnlyVisible => IsNoAuction ? Visibility.Visible : Visibility.Hidden;
        public bool IsNoTrade => Flags.HasFlag(ItemFlags.FLAG_NoTrade);
        public Visibility IsNoTradeVisible => IsNoTrade ? Visibility.Visible : Visibility.Hidden;
        public bool IsRetired => Flags.HasFlag(ItemFlags.FLAG_Retired);
        public Visibility IsRetiredVisible => IsRetired ? Visibility.Visible : Visibility.Hidden;
        public bool IsCrafted => Flags.HasFlag(ItemFlags.FLAG_Crafted);
        public Visibility IsCraftedVisible => IsCrafted ? Visibility.Visible : Visibility.Hidden;
        public bool IsPVPOnly => Flags.HasFlag(ItemFlags.FLAG_PVPOnly);
        public Visibility IsPVPOnlyVisible => IsPVPOnly ? Visibility.Visible : Visibility.Hidden;
        public bool IsNoPVP => Flags.HasFlag(ItemFlags.FLAG_NoPVP);
        public Visibility IsNoPVPVisible => IsNoPVP ? Visibility.Visible : Visibility.Hidden;


        private static Dictionary<string, BitmapImage> StatImages = new Dictionary<string, BitmapImage>()
        {
            { "Health",         new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Health.png", UriKind.Absolute)) },
            { "Mana",           new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Mana.png", UriKind.Absolute)) },
            { "Energy",         new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Energy.png", UriKind.Absolute)) },
            { "PowerPip",       new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Power_Pip.png", UriKind.Absolute)) },
            { "Pip",            new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Pip.png", UriKind.Absolute)) },
            { "Accuracy",       new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Accuracy.png", UriKind.Absolute)) },
            { "Archmastery",    new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Archmastery.png", UriKind.Absolute)) },
            { "ArmorPiercing",  new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Armor_Piercing.png", UriKind.Absolute)) },
            { "Block",          new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Critical_Block.png", UriKind.Absolute)) },
            { "Critical",       new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Critical.png", UriKind.Absolute)) },
            { "Damage",         new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Damage.png", UriKind.Absolute)) },
            { "FishingLuck",    new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Fishing_Luck.png", UriKind.Absolute)) },
            { "FlatDamage",     new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Flat_Damage.png", UriKind.Absolute)) },
            { "FlatResistance", new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Flat_Resistance.png", UriKind.Absolute)) },
            { "Healing",        new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Healing.png", UriKind.Absolute)) },
            { "Incoming",       new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Incoming.png", UriKind.Absolute)) },
            { "Outgoing",       new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Outgoing.png", UriKind.Absolute)) },
            { "PipConversion",  new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Pip_Conversion.png", UriKind.Absolute)) },
            { "Resistance",     new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Resistance.png", UriKind.Absolute)) },
            { "Shadowpip",      new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Shadow_Pip.png", UriKind.Absolute)) },
            { "StunResistance", new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Stun_Resistance.png", UriKind.Absolute)) },
            { "Fire",           new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_School_Fire.png", UriKind.Absolute)) },
            { "Ice",            new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_School_Ice.png", UriKind.Absolute)) },
            { "Storm",          new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_School_Storm.png", UriKind.Absolute)) },
            { "Myth",           new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_School_Myth.png", UriKind.Absolute)) },
            { "Life",           new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_School_Life.png", UriKind.Absolute)) },
            { "Death",          new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_School_Death.png", UriKind.Absolute)) },
            { "Balance",        new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_School_Balance.png", UriKind.Absolute)) },
            { "Shadow",         new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_School_Shadow.png", UriKind.Absolute)) },
            { "Universal",      new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_School_Global.png", UriKind.Absolute)) },
            { "TearJewel",      new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Equipment_TearJewel.png", UriKind.Absolute)) },
            { "CircleJewel",      new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Equipment_CircleJewel.png", UriKind.Absolute)) },
            { "SquareJewel",      new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Equipment_SquareJewel.png", UriKind.Absolute)) },
            { "TriangleJewel",      new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Equipment_TriangleJewel.png", UriKind.Absolute)) },
            { "PowerPin",      new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Equipment_PinSquarePower.png", UriKind.Absolute)) },
            { "ShieldPin",      new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Equipment_PinSquareShield.png", UriKind.Absolute)) },
            { "SwordPin",      new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Equipment_PinSquareSword.png", UriKind.Absolute)) },
            { "SpeedBonus",      new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_SpeedBonus.png", UriKind.Absolute)) },
            { "CrownsOnly",      new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Flag_CrownsOnly.png", UriKind.Absolute)) },
            { "NoAuction",      new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Flag_NoAuction.png", UriKind.Absolute)) },
            { "NoHatchmaking",      new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Flag_NoHatchmaking.png", UriKind.Absolute)) },
            { "NoPVP",      new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Flag_NoPVP.png", UriKind.Absolute)) },
            { "PVPOnly",      new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Flag_PVPOnly.png", UriKind.Absolute)) },
            { "NoTrade",      new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Flag_NoTrade.png", UriKind.Absolute)) },
        };

        public TextBlock GetStatDisplay(bool showIDs)
        {
            void AddSingle(TextBlock tb, string before, BitmapImage img1 = null, BitmapImage img2 = null, string after = null)
            {
                if (!string.IsNullOrEmpty(before))
                {
                    Run run = new Run(before);
                    tb.Inlines.Add(run);
                }
                if (img1 != null)
                {
                    Image img = new Image();
                    img.Source = img1;
                    img.Width = 15;
                    img.Height = 15;
                    InlineUIContainer iuc = new InlineUIContainer(img);
                    iuc.BaselineAlignment = BaselineAlignment.Center;
                    tb.Inlines.Add(iuc);
                }
                if (img2 != null)
                {
                    Image img = new Image();
                    img.Source = img2;
                    img.Width = 15;
                    img.Height = 15;
                    InlineUIContainer iuc = new InlineUIContainer(img);
                    iuc.BaselineAlignment = BaselineAlignment.Center;
                    tb.Inlines.Add(iuc);
                }
                if (!string.IsNullOrEmpty(after))
                {
                    Run run = new Run(after);
                    tb.Inlines.Add(run);
                }
            }

            void AddIcons(TextBlock tb, List<BitmapImage> images)
            {
                foreach (var image in images)
                {
                    Image img = new Image();
                    img.Source = image;
                    img.Width = 15;
                    img.Height = 15;
                    InlineUIContainer iuc = new InlineUIContainer(img);
                    iuc.BaselineAlignment = BaselineAlignment.Center;
                    tb.Inlines.Add(iuc);
                }
            }

            TextBlock tb = new TextBlock();

            if (showIDs)
            {
                AddSingle(tb, $"ID: {ID.ToString().ToUpper()} ", null, null, "\n");
            }

            if (MaxHealth > 0) AddSingle(tb, $"+{MaxHealth} Max ", StatImages["Health"], null, "\n");
            if (MaxMana > 0) AddSingle(tb, $"+{MaxMana} Max ", StatImages["Mana"], null, "\n");
            if (MaxEnergy > 0) AddSingle(tb, $"+{MaxEnergy} Max ", StatImages["Energy"], null, "\n");
            if (PowerpipChance > 0) AddSingle(tb, $"+{PowerpipChance}% ", StatImages["PowerPip"], null, " Chance\n");

            if (FishingLuck > 0) AddSingle(tb, $"+{FishingLuck}% ", StatImages["FishingLuck"], null, "\n");

            foreach (var pair in Accuracies)
            {
                AddSingle(tb, $"+{pair.Value}% ", StatImages[pair.Key.ToString()], StatImages["Accuracy"], "\n");
            }
            foreach (var pair in Criticals)
            {
                AddSingle(tb, $"+{pair.Value} ", StatImages[pair.Key.ToString()], StatImages["Critical"], " Rating\n");
            }
            foreach (var pair in Blocks)
            {
                AddSingle(tb, $"+{pair.Value} ", StatImages[pair.Key.ToString()], StatImages["Block"], " Rating\n");
            }
            foreach (var pair in Damages)
            {
                AddSingle(tb, $"+{pair.Value} ", StatImages[pair.Key.ToString()], StatImages["Damage"], "\n");
            }
            foreach (var pair in FlatDamages)
            {
                AddSingle(tb, $"+{pair.Value} ", StatImages[pair.Key.ToString()], StatImages["FlatDamage"], "\n");
            }
            foreach (var pair in Resists)
            {
                AddSingle(tb, $"+{pair.Value} ", StatImages[pair.Key.ToString()], StatImages["Resistance"], "\n");
            }
            foreach (var pair in FlatResists)
            {
                AddSingle(tb, $"+{pair.Value} ", StatImages[pair.Key.ToString()], StatImages["FlatResistance"], "\n");
            }
            foreach (var pair in Pierces)
            {
                AddSingle(tb, $"+{pair.Value} ", StatImages[pair.Key.ToString()], StatImages["ArmorPiercing"], "\n");
            }
            foreach (var pair in PipConversions)
            {
                AddSingle(tb, $"+{pair.Value} ", StatImages[pair.Key.ToString()], StatImages["PipConversion"], " Rating\n");
            }

            if (OutgoingHealing > 0) AddSingle(tb, $"+{OutgoingHealing}% ", StatImages["Outgoing"], StatImages["Healing"], "\n");
            if (IncomingHealing > 0) AddSingle(tb, $"+{IncomingHealing}% ", StatImages["Incoming"], StatImages["Healing"], "\n");

            if (PowerpipsGiven > 0) AddSingle(tb, $"+{PowerpipsGiven} ", StatImages["PowerPip"], null, "\n");
            if (PipsGiven > 0) AddSingle(tb, $"+{PipsGiven} ", StatImages["Pip"], null, "\n");
            if (ShadowpipRating > 0) AddSingle(tb, $"+{ShadowpipRating} ", StatImages["Shadowpip"], null, " Rating\n");
            if (StunResistChance > 0) AddSingle(tb, $"+{StunResistChance}% ", StatImages["StunResistance"], null, "\n");
            if (ArchmasteryRating > 0) AddSingle(tb, $"+{ArchmasteryRating} ", StatImages["Archmastery"], null, " Rating\n");

            if (SpeedBonus > 0) AddSingle(tb, $"+{SpeedBonus}% ", StatImages["SpeedBonus"], null, "\n");

            if (TotalSlots > 0)
            {
                AddSingle(tb, "\nSockets", null, null, "\n");
                for (int i = 0; i < TearJewelSlots; i++) AddSingle(tb, null, StatImages["TearJewel"], null, " (Tear)\n");
                for (int i = 0; i < CircleJewelSlots; i++) AddSingle(tb, null, StatImages["CircleJewel"], null, " (Circle)\n");
                for (int i = 0; i < SquareJewelSlots; i++) AddSingle(tb, null, StatImages["SquareJewel"], null, " (Square)\n");
                for (int i = 0; i < TriangleJewelSlots; i++) AddSingle(tb, null, StatImages["TriangleJewel"], null, " (Triangle)\n");
                for (int i = 0; i < PowerPinSlots; i++) AddSingle(tb, null, StatImages["PowerPin"], null, " (Power)\n");
                for (int i = 0; i < ShieldPinSlots; i++) AddSingle(tb, null, StatImages["ShieldPin"], null, " (Shield)\n");
                for (int i = 0; i < SwordPinSlots; i++) AddSingle(tb, null, StatImages["SwordPin"], null, " (Sword)\n");
                AddSingle(tb, null, null, null, "\n");
            }

            // TODO: Spells

            if (SchoolRequirement != School.Any) AddSingle(tb, null, StatImages[SchoolRequirement.ToString()], null, " School Only\n");
            if (LevelRequirement > 1) AddSingle(tb, $"Level {LevelRequirement}+ Only", null, null, "\n");

            List<BitmapImage> icons = new List<BitmapImage>();
            if (IsCrownsOnly) icons.Add(StatImages["CrownsOnly"]);
            if (IsNoAuction) icons.Add(StatImages["NoAuction"]);
            if (IsNoTrade) icons.Add(StatImages["NoTrade"]);
            //if (false) icons.Add(StatImages["NoHatchmaking"]);
            if (IsNoPVP) icons.Add(StatImages["NoPVP"]);
            if (IsPVPOnly) icons.Add(StatImages["PVPOnly"]);
            AddIcons(tb, icons);

            if (IsRetired) AddSingle(tb, "RETIRED ITEM", null, null, "\n");

            return tb;
        }
        public int TotalSlots
        {
            get
            {
                return (TearJewelSlots +
                    CircleJewelSlots +
                    SquareJewelSlots +
                    TriangleJewelSlots +
                    PowerPinSlots +
                    ShieldPinSlots +
                    SwordPinSlots);
            }
        }

        public ItemDisplay()
        {
            backingItem = new Item();
        }
        public ItemDisplay(Item item)
        {
            backingItem = item;
        }
        public static explicit operator ItemDisplay(Item i)
        {
            return new ItemDisplay(i);
        }
        public static ItemDisplay MakeRandomItem()
        {
            Random rand = new Random((int)System.DateTime.Now.ToUniversalTime().Ticks);
            ItemDisplay item = new ItemDisplay()
            {
                MaxHealth = rand.Next(0, 2) == 1 ? rand.Next(1, 501) : 0,
                MaxMana = rand.Next(0, 2) == 1 ? rand.Next(1, 501) : 0,
                MaxEnergy = rand.Next(0, 2) == 1 ? rand.Next(1, 501) : 0,
                ArchmasteryRating = rand.Next(0, 2) == 1 ? rand.Next(1, 501) : 0,
                FishingLuck = rand.Next(0, 2) == 1 ? rand.Next(1, 101) : 0,
                IncomingHealing = rand.Next(0, 2) == 1 ? rand.Next(1, 101) : 0,
                OutgoingHealing = rand.Next(0, 2) == 1 ? rand.Next(1, 101) : 0,
                LevelRequirement = rand.Next(0, 2) == 1 ? rand.Next(1, 17) * 10 : 1,
                SchoolRequirement = (School)rand.Next((int)School.Any, (int)School.Shadow),
                PowerpipChance = rand.Next(0, 2) == 1 ? rand.Next(1, 22) : 1,
                ShadowpipRating = rand.Next(0, 2) == 1 ? rand.Next(1, 21) : 1,
                StunResistChance = rand.Next(0, 101),
                Type = (ItemType)rand.Next((int)ItemType.Hat, (int)ItemType.ItemSetBonusData),
                TearJewelSlots = rand.Next(0, 3),
                CircleJewelSlots = rand.Next(0, 3),
                SquareJewelSlots = rand.Next(0, 3),
                TriangleJewelSlots = rand.Next(0, 3),
                PowerPinSlots = rand.Next(0, 3),
                ShieldPinSlots = rand.Next(0, 3),
                SwordPinSlots = rand.Next(0, 3),
            };

            item.SchoolRestriction = item.SchoolRequirement != School.Any ? School.Any : (School)rand.Next((int)School.Any, (int)School.Shadow);
            item.Accuracies.Add(rand.Next(0, 2) == 0 ? School.Any : item.SchoolRequirement, rand.Next(1, 20));
            item.Blocks.Add(rand.Next(0, 2) == 0 ? School.Any : item.SchoolRequirement, rand.Next(1, 20));
            item.Damages.Add(rand.Next(0, 2) == 0 ? School.Any : item.SchoolRequirement, rand.Next(1, 20));
            item.Criticals.Add(rand.Next(0, 2) == 0 ? School.Any : item.SchoolRequirement, rand.Next(1, 20));
            item.FlatDamages.Add(rand.Next(0, 2) == 0 ? School.Any : item.SchoolRequirement, rand.Next(1, 20));
            item.FlatResists.Add(rand.Next(0, 2) == 0 ? School.Any : item.SchoolRequirement, rand.Next(1, 20));
            item.Pierces.Add(rand.Next(0, 2) == 0 ? School.Any : item.SchoolRequirement, rand.Next(1, 20));
            item.PipConversions.Add(rand.Next(0, 2) == 0 ? School.Any : item.SchoolRequirement, rand.Next(1, 20));
            item.Resists.Add(rand.Next(0, 2) == 0 ? School.Any : item.SchoolRequirement, rand.Next(1, 20));


            string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
            string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
            string name = "";
            name += consonants[rand.Next(consonants.Length)].ToUpper();
            name += vowels[rand.Next(vowels.Length)];
            int b = rand.Next(8, 31); //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
            while (b > 0)
            {
                name += consonants[rand.Next(consonants.Length)];
                b--;
                name += vowels[rand.Next(vowels.Length)];
                b--;
            }

            item.Name = name;

            return item;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }

    public class ItemFilter
    {
        public string Name { get; set; } = string.Empty;
        public Item.ItemType Type { get; set; } = Item.ItemType.None;
        public Item.School School { get; set; } = Item.School.None;
        public int MinLevel { get; set; } = 0;
        public int MaxLevel { get; set; } = 160;
    }

    public class ItemLoadout
    {
        protected List<ItemDisplay> EquippedItems;

        public Item.School WizardSchool { get; set; }
        public int WizardLevel { get; set; }

        public ItemDisplay CustomStats = null;
        public bool UseCustomStats = false;

        private static Dictionary<FileLevelStats, int[]> ConstantStatValues;
        public ItemLoadout()
        {
            EquippedItems = new List<ItemDisplay>();

            if (ConstantStatValues == null)
            {
                ConstantStatValues = new Dictionary<FileLevelStats, int[]>((int)FileLevelStats._Count);

                ReadLevelStats();
            }
        }
        enum FileLevelStats
        {
            Level,
            Fire,
            Ice,
            Storm,
            Myth,
            Life,
            Death,
            Balance,
            Mana,
            Pip,
            Energy,
            Shadow,
            Arch,
            _Count
        }
        private static void ReadLevelStats()
        {
            for (FileLevelStats i = 0; i < FileLevelStats._Count; i++)
            {
                ConstantStatValues[i] = new int[160];
            }

            using StreamReader sr = File.OpenText("Assets/Data/level_based_stats.csv");
            string line;
            line = sr.ReadLine(); // Discard header.

            for (int i = 0; (line = sr.ReadLine()) != null; i++)
            {
                string[] X = line.Split(',');

                for (int j = 0; j < X.Length; j++)
                {
                    ConstantStatValues[(FileLevelStats)j][i] = int.Parse(X[j]);
                }
            }
        }
        private int GetCurrentJewelSlots(Item.ItemType jewelType)
        {
            int jewelSlots = 0;
            foreach (ItemDisplay item in EquippedItems)
            {
                // Skip jewel slots. Jewels shouldn't have jewel slots. Prevents odd dependencies with jewel slots.
                if (item.Type >= ItemType.TearJewel && item.Type <= ItemType.PinSquareSword)
                {
                    continue;
                }
                switch (jewelType)
                {
                    case ItemType.TearJewel:
                        jewelSlots += item.TearJewelSlots;
                        break;
                    case ItemType.CircleJewel:
                        jewelSlots += item.CircleJewelSlots;
                        break;
                    case ItemType.SquareJewel:
                        jewelSlots += item.SquareJewelSlots;
                        break;
                    case ItemType.TriangleJewel:
                        jewelSlots += item.TriangleJewelSlots;
                        break;
                    case ItemType.PowerPin:
                        jewelSlots += item.PowerPinSlots;
                        break;
                    case ItemType.ShieldPin:
                        jewelSlots += item.ShieldPinSlots;
                        break;
                    case ItemType.SwordPin:
                        jewelSlots += item.SwordPinSlots;
                        break;
                    default:
                        return -1;
                }
            }
            return jewelSlots;
        }
        public int GetNumberAllowedEquipped(Item.ItemType type)
        {
            switch (type)
            {
                case ItemType.Hat:
                case ItemType.Robe:
                case ItemType.Shoes:
                case ItemType.Weapon:
                case ItemType.Athame:
                case ItemType.Amulet:
                case ItemType.Ring:
                case ItemType.Deck:
                case ItemType.Pet:
                case ItemType.Mount:
                    return 1;
                case ItemType.TearJewel:
                case ItemType.CircleJewel:
                case ItemType.SquareJewel:
                case ItemType.TriangleJewel:
                case ItemType.PowerPin:
                case ItemType.ShieldPin:
                case ItemType.SwordPin:
                    return GetCurrentJewelSlots(type);
                case ItemType.ItemSetBonusData:
                    return int.MaxValue;
                default:
                    return -1;
            }
        }
        public int GetNumberOfEquipped(Item.ItemType type)
        {
            return EquippedItems.Count(i => i.Type == type);
        }
        public ItemDisplay GetEquippedFromType(Item.ItemType type)
        {
            return EquippedItems.FirstOrDefault(i => i.Type == type);
        }
        public void EquipItem(ItemDisplay item)
        {
            // Use EquipJewel or CalculateItemSetBonus for this...
            if (item.Type > ItemType.Mount) return;

            if (GetNumberOfEquipped(item.Type) >= GetNumberAllowedEquipped(item.Type))
            {
                EquippedItems.Remove(GetEquippedFromType(item.Type));
            }

            EquippedItems.Add(item);
        }
        public void UnequipItem(Item.ItemType type)
        {
            ItemDisplay equipped = EquippedItems.FirstOrDefault(i => i.Type == type);
            if (equipped != null)
            {
                EquippedItems.Remove(equipped);
            }
        }
        public ItemDisplay CalculateStats()
        {
            
            static void SumDicts<T>(Dictionary<T, int> a, Dictionary<T, int> b)
            {
                foreach (var pair in b)
                {
                    if (a.ContainsKey(pair.Key))
                    {
                        a[pair.Key] += pair.Value;
                    }
                    else
                    {
                        a.Add(pair.Key, pair.Value);
                    }
                }
            }

            ItemDisplay Stats = new ItemDisplay();

            if (!UseCustomStats)
            {
                if (WizardLevel > 0)
                {
                    if (WizardSchool != School.Any)
                    {
                        Stats.MaxHealth = ConstantStatValues[(FileLevelStats)WizardSchool][WizardLevel - 1];
                    }
                    Stats.MaxMana = ConstantStatValues[FileLevelStats.Mana][WizardLevel - 1];
                    Stats.MaxEnergy = ConstantStatValues[FileLevelStats.Energy][WizardLevel - 1];
                    Stats.PowerpipChance = ConstantStatValues[FileLevelStats.Pip][WizardLevel - 1];
                    Stats.ShadowpipRating = ConstantStatValues[FileLevelStats.Shadow][WizardLevel - 1];
                    Stats.ArchmasteryRating = ConstantStatValues[FileLevelStats.Arch][WizardLevel - 1];
                }
            }

            foreach (ItemDisplay item in EquippedItems)
            {
                Stats.MaxHealth += item.MaxHealth;
                Stats.MaxMana += item.MaxMana;
                Stats.MaxEnergy += item.MaxEnergy;

                Stats.PowerpipChance += item.PowerpipChance;
                Stats.ShadowpipRating += item.ShadowpipRating;
                Stats.ArchmasteryRating += item.ArchmasteryRating;
                Stats.FishingLuck += item.FishingLuck;
                Stats.IncomingHealing += item.IncomingHealing;
                Stats.OutgoingHealing += item.OutgoingHealing;
                Stats.PipsGiven += item.PipsGiven;
                Stats.PowerpipsGiven += item.PowerpipsGiven;
                Stats.SpeedBonus += item.SpeedBonus;
                Stats.StunResistChance += item.StunResistChance;

                SumDicts(Stats.Damages, item.Damages);
                SumDicts(Stats.FlatDamages, item.FlatDamages);
                SumDicts(Stats.Resists, item.Resists);
                SumDicts(Stats.FlatResists, item.FlatResists);
                SumDicts(Stats.Accuracies, item.Accuracies);
                SumDicts(Stats.Pierces, item.Pierces);
                SumDicts(Stats.Criticals, item.Criticals);
                SumDicts(Stats.Blocks, item.Blocks);
                SumDicts(Stats.PipConversions, item.PipConversions);
                SumDicts(Stats.ItemCards, item.ItemCards);
            }

            return Stats;
        }
    }
}