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
            item.FlatResists.Add(rand.Next(0, 2) == 0 ? School.Any : item.SchoolRequirement, rand.Next(1, 20));


            string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
            string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
            string name = "";
            name += consonants[rand.Next(consonants.Length)].ToUpper();
            name += vowels[rand.Next(vowels.Length)];
            int b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
            while (b < rand.Next(8, 31))
            {
                name += consonants[rand.Next(consonants.Length)];
                b++;
                name += vowels[rand.Next(vowels.Length)];
                b++;
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
        public void EquipItem(ItemDisplay item)
        {
            if (GetNumberOfEquipped(item.Type) >= GetNumberAllowedEquipped(item.Type))
            {
                return;
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
                Stats.MaxHealth = ConstantStatValues[(FileLevelStats)WizardSchool + 1][WizardLevel];
                Stats.MaxMana = ConstantStatValues[FileLevelStats.Mana][WizardLevel];
                Stats.MaxEnergy = ConstantStatValues[FileLevelStats.Energy][WizardLevel];
                Stats.PowerpipChance = ConstantStatValues[FileLevelStats.Pip][WizardLevel];
                Stats.ShadowpipRating = ConstantStatValues[FileLevelStats.Shadow][WizardLevel];
                Stats.ArchmasteryRating = ConstantStatValues[FileLevelStats.Arch][WizardLevel];
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