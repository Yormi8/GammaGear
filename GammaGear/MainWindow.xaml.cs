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
using GammaGear.Source;

namespace GammaGear
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ItemFilter DBItemFilters = new ItemFilter();
        private ItemLoadout mainLoadout = new ItemLoadout();
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
        private void OnSelectDatabaseTabItem(object sender, ExecutedRoutedEventArgs e)
        {
            DatabaseTabItem.IsSelected = true;
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
            TextBlock tb = item.GetStatDisplay();
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
                DBEquippedItemName.Text = mainLoadout.GetEquippedFromType(item.Type).Name;
                SetItemContent(mainLoadout.GetEquippedFromType(item.Type), DBEquippedItemContent);
            }
            else
            {
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
                DamageTextBoxes[i].Text =           (Output.Damages.GetValueOrDefault((Item.School)i) +         Output.Damages.GetValueOrDefault(Item.School.Universal)).ToString(fp);
                FlatDamageTextBoxes[i].Text =       (Output.FlatDamages.GetValueOrDefault((Item.School)i) +     Output.FlatDamages.GetValueOrDefault(Item.School.Universal)).ToString(fw);
                ResistTextBoxes[i].Text =           (Output.Resists.GetValueOrDefault((Item.School)i) +         Output.Resists.GetValueOrDefault(Item.School.Universal)).ToString(fp);
                FlatResistanceTextBoxes[i].Text =   (Output.FlatResists.GetValueOrDefault((Item.School)i) +     Output.FlatResists.GetValueOrDefault(Item.School.Universal)).ToString(fw);
                AccuracyTextBoxes[i].Text =         (Output.Accuracies.GetValueOrDefault((Item.School)i) +      Output.Accuracies.GetValueOrDefault(Item.School.Universal)).ToString(fp);
                CriticalTextBoxes[i].Text =         (Output.Criticals.GetValueOrDefault((Item.School)i) +       Output.Criticals.GetValueOrDefault(Item.School.Universal)).ToString(fw);
                CriticalBlockTextBoxes[i].Text =    (Output.Blocks.GetValueOrDefault((Item.School)i) +          Output.Blocks.GetValueOrDefault(Item.School.Universal)).ToString(fw);
                PierceTextBoxes[i].Text =           (Output.Pierces.GetValueOrDefault((Item.School)i) +         Output.Pierces.GetValueOrDefault(Item.School.Universal)).ToString(fp);
                PipConversionTextBoxes[i].Text =    (Output.PipConversions.GetValueOrDefault((Item.School)i) +  Output.PipConversions.GetValueOrDefault(Item.School.Universal)).ToString(fw);
            }
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
            ItemDisplay selectedItem = ItemDatabaseGrid.SelectedItem as ItemDisplay;

            mainLoadout.EquipItem(selectedItem);

            DBItemSelected(ItemDatabaseGrid, null);
        }
        private void UpdateStatsTabEvent(object sender, RoutedEventArgs eventArgs)
        {
            if (this.IsVisible)
            {
                UpdateStatsTab();
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

        public static readonly RoutedUICommand SelectTabItem = new RoutedUICommand
        (
            "Selects a tab item.",
            "SelectTabItem",
            typeof(W101Commands)
        );
    }

    public class ItemViewModel : ObservableCollection<ItemDisplay>
    {
        public ItemDisplay SelectedItem { get; set; }

        public ItemViewModel()
        {
            for (int i = 0; i < 1000; i++)
            {
                this.Add(ItemDisplay.MakeRandomItem());
            }

            this.Add(new ItemDisplay() 
            { 
                Name = "Malistaire's Cloak of Flux", 
                Type = Item.ItemType.Robe, 
                SchoolRequirement = Item.School.Storm 
            });
            this.Add(new ItemDisplay()
            {
                Name = "gaboty",
                Type = Item.ItemType.Robe,
                SchoolRequirement = Item.School.Storm
            });
        }
    }

    public class ItemDisplay : Item, INotifyPropertyChanged
    {
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
            _ => "Assets/Images/(Icon)_Equipment_" + Type.ToString() + ".png"
        };

        public string DisplaySchool => SchoolRequirement switch
        {
            School.Universal => SchoolRestriction != School.Universal ? 
            "Not " + SchoolRestriction.ToString() : 
            "Any",
            _ => SchoolRequirement.ToString()
        };

        public string DisplaySchoolSource => SchoolRequirement switch
        {
            School.Universal => SchoolRestriction != School.Universal ?
            "Assets/Images/(Icon)_School_Not_" + SchoolRestriction.ToString() + ".png" :
            "Assets/Images/(Icon)_School_Global.png",
            _ => "Assets/Images/(Icon)_School_" + SchoolRequirement.ToString() + ".png"
        };
        private static Dictionary<string, BitmapImage> StatImages = new Dictionary<string, BitmapImage>()
        {
            { "Health",         new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Health.png", UriKind.Absolute)) },
            { "Mana",           new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Mana.png", UriKind.Absolute)) },
            { "Energy",         new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Energy.png", UriKind.Absolute)) },
            { "PowerPip",       new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Power_Pip.png", UriKind.Absolute)) },
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
            { "ShadowPip",      new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Shadow_Pip.png", UriKind.Absolute)) },
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
        };

        public TextBlock GetStatDisplay()
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
                    tb.Inlines.Add(iuc);
                }
                if (img2 != null)
                {
                    Image img = new Image();
                    img.Source = img2;
                    img.Width = 15;
                    img.Height = 15;
                    InlineUIContainer iuc = new InlineUIContainer(img);
                    tb.Inlines.Add(iuc);
                }
                if (!string.IsNullOrEmpty(after))
                {
                    Run run = new Run(after);
                    tb.Inlines.Add(run);
                }
            }

            TextBlock tb = new TextBlock();

            if (MaxHealth > 0) AddSingle(tb, $"+{MaxHealth} Max", StatImages["Health"], null, "\n");
            if (MaxMana > 0) AddSingle(tb, $"+{MaxMana} Max", StatImages["Mana"], null, "\n");
            if (MaxEnergy > 0) AddSingle(tb, $"+{MaxEnergy} Max", StatImages["Energy"], null, "\n");
            if (PowerpipChance> 0) AddSingle(tb, $"+{PowerpipChance}%", StatImages["PowerPip"], null, "Chance\n");

            return tb;
        }


        public static ItemDisplay MakeRandomItem()
        {
            Random rand = new Random((int)System.DateTime.Now.ToUniversalTime().Ticks);
            ItemDisplay item = new ItemDisplay
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
                Type = (ItemType)rand.Next((int)ItemType.Hat, (int)ItemType.ItemSetBonusData),
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
                if (item.Type >= Item.ItemType.TearJewel && item.Type <= Item.ItemType.PinSquareSword)
                {
                    continue;
                }
                switch (jewelType)
                {
                    case Item.ItemType.TearJewel:
                        jewelSlots += item.TearJewelSlots;
                        break;
                    case Item.ItemType.CircleJewel:
                        jewelSlots += item.CircleJewelSlots;
                        break;
                    case Item.ItemType.SquareJewel:
                        jewelSlots += item.SquareJewelSlots;
                        break;
                    case Item.ItemType.TriangleJewel:
                        jewelSlots += item.TriangleJewelSlots;
                        break;
                    case Item.ItemType.PowerPin:
                        jewelSlots += item.PowerPinSlots;
                        break;
                    case Item.ItemType.ShieldPin:
                        jewelSlots += item.ShieldPinSlots;
                        break;
                    case Item.ItemType.SwordPin:
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
                case Item.ItemType.Hat:
                case Item.ItemType.Robe:
                case Item.ItemType.Shoes:
                case Item.ItemType.Weapon:
                case Item.ItemType.Athame:
                case Item.ItemType.Amulet:
                case Item.ItemType.Ring:
                case Item.ItemType.Deck:
                case Item.ItemType.Pet:
                case Item.ItemType.Mount:
                    return 1;
                case Item.ItemType.TearJewel:
                case Item.ItemType.CircleJewel:
                case Item.ItemType.SquareJewel:
                case Item.ItemType.TriangleJewel:
                case Item.ItemType.PowerPin:
                case Item.ItemType.ShieldPin:
                case Item.ItemType.SwordPin:
                    return GetCurrentJewelSlots(type);
                case Item.ItemType.ItemSetBonusData:
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
            return EquippedItems.First(i => i.Type == type);
        }
        public void EquipItem(ItemDisplay item)
        {
            // Use EquipJewel or CalculateItemSetBonus for this...
            if (item.Type > Item.ItemType.Mount) return;

            if (GetNumberOfEquipped(item.Type) >= GetNumberAllowedEquipped(item.Type))
            {
                EquippedItems.Remove(GetEquippedFromType(item.Type));
            }

            EquippedItems.Add(item);
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
                    if (WizardSchool != Item.School.Any)
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