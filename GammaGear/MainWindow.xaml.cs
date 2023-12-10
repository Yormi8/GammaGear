using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
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
using GammaGear.Source.Database;
using Microsoft.Win32;
using FolderBrowserDialog = System.Windows.Forms.FolderBrowserDialog;
using FormsDialogResult = System.Windows.Forms.DialogResult;
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

        // Main Tab Variables
        private ItemType SelectedBaseItemType = ItemType.None;
        private int SelectedBaseItemSocket = 0;
        private ItemType SelectedBaseItemSocketTarget = ItemType.None;
        public MainWindow()
        {
            InitializeComponent();

            StateChanged += MainWindowStateChangeRaised;

            ThemeButtons = new MenuItem[]
            {
                LatteThemeButton,
                FrappeThemeButton,
                MacchiatoThemeButton,
                MochaThemeButton
            };

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
        // Can execute
        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        // Minimize
        private void CommandBinding_Executed_Minimize(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        // Maximize
        private void CommandBinding_Executed_Maximize(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
        }

        // Restore
        private void CommandBinding_Executed_Restore(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }

        // Close
        private void CommandBinding_Executed_Close(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }

        // State change
        private void MainWindowStateChangeRaised(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                MainWindowPanel.Margin = new Thickness(8);
                WindowRestoreButton.Visibility = Visibility.Visible;
                WindowMaximizeButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                MainWindowPanel.Margin = new Thickness(0);
                WindowRestoreButton.Visibility = Visibility.Collapsed;
                WindowMaximizeButton.Visibility = Visibility.Visible;
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
        private void On_StatsTab_BaseItemButton_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                element.IsVisibleChanged += On_StatsTab_BaseItemButton_VisibleChanged;
            }
        }
        private void On_StatsTab_BaseItemButton_Click(object sender, RoutedEventArgs e)
        {
            void IterateJewels(ItemDisplay item, ItemType jewelType)
            {
                for (int i = 0; i < item.GetJewelSlots(jewelType); i++)
                {
                    Grid g = new Grid()
                    {
                        Children =
                        {
                            new Image()
                            {
                                Source = ItemDisplay.StatImages[jewelType.ToString() + (mainLoadout.GetEquippedFromType(item.Type, WizardSelectedItemDetails.Children.Count + 1) != null ? "Filled" : "")]
                            }
                        }
                    };
                    ItemDisplay jewel = mainLoadout.GetEquippedFromType(item.Type, WizardSelectedItemDetails.Children.Count + 1);
                    Button b = new Button()
                    {
                        Width = 25,
                        Height = 25,
                        BorderThickness = new Thickness(0, 0, 0, 0),
                        Padding = new Thickness(0, 0, 0, 0),
                        Margin = new Thickness(3, 3, 3, 3),
                        Background = null,
                        Content = g,
                        Tag = new Tuple<ItemType, int>(jewelType, WizardSelectedItemDetails.Children.Count + 1),
                        ToolTip = jewel != null ? jewel.GetStatDisplay(mainLoadout, true, showDBItemIDs) : "Equip Jewel"
                    };
                    b.Click += On_StatsTab_BaseItemButtonEquipNew_Click;
                    WizardSelectedItemDetails.Children.Add(b);
                }
            }

            // Update the display
            if (sender is ItemType itemType ||
                (sender is Button button &&
                button.Tag is string itemTypeString &&
                Enum.TryParse(itemTypeString, true, out itemType)))
            {
                SelectedBaseItemType = itemType;
                SelectedBaseItemSocketTarget = itemType;
                SelectedBaseItemSocket = 0;
                ItemDisplay item = mainLoadout.GetEquippedFromType(itemType);
                WizardSelectedItemDetails.Children.Clear();
                if (item != null)
                {
                    SetItemContent(item, WizardSelectedItemDisplay, true);
                    for (ItemType i = ItemType.TearJewel; i <= ItemType.SwordPin; i++)
                    {
                        IterateJewels(item, i);
                    }
                }
                else
                {
                    SetItemContent(null, WizardSelectedItemDisplay, true);
                    On_StatsTab_BaseItemButtonEquipNew_Click(sender, e);
                }
            }
        }
        private void On_StatsTab_BaseItemButtonEquipNew_Click(object sender, RoutedEventArgs e)
        {
            DatabaseTabItem.IsSelected = true;
            DBTypeBox.SelectedIndex = (int)SelectedBaseItemType + 1;
            if (sender is Button b && b.Tag is Tuple<ItemType, int>(ItemType type, int socket))
            {
                DBTypeBox.SelectedIndex = (int)type + 1;
                SelectedBaseItemType = type;
                SelectedBaseItemSocket = socket;
            }
            DBSearchButtonOnClick(null, null);
        }
        private void On_StatsTab_BaseItemButton_VisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Button button &&
                button.IsVisible &&
                button.Tag is string itemTypeString &&
                Enum.TryParse(itemTypeString, true, out ItemType itemType))
            {
                button.Content = mainLoadout.GetEquippedFromType(itemType)?.Name ?? "None";
                button.ToolTip = mainLoadout.GetEquippedFromType(itemType)?.GetStatDisplay(mainLoadout, true, showDBItemIDs) ?? null;
            }
        }
        private void OnSelectDatabaseTabItem(object sender, RoutedEventArgs e)
        {
            DatabaseTabItem.IsSelected = true;
            if (sender is Button button &&
                button.Tag is string itemTypeString &&
                Enum.TryParse(itemTypeString, true, out ItemType itemType))
            {
                DBTypeBox.SelectedIndex = (int)itemType + 1;
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
        private void SetItemContent(ItemDisplay item, StackPanel parent, bool showName)
        {
            parent.Children.Clear();
            TextBlock tb = item?.GetStatDisplay(mainLoadout, showName, showDBItemIDs) ??
                           new TextBlock()
                           {
                               Text = "None",
                               FontWeight = FontWeights.Bold,
                               Margin = new Thickness(3, 3, 3, 0)
                           }; ;
            parent.Children.Add(tb);
        }
        private void DBItemSelected(object sender, RoutedEventArgs eventArgs)
        {
            if (!IsVisible) return;
            if ((sender as DataGrid).SelectedItem is not ItemDisplay item) return;

            DBSelectedItemName.Text = item.Name;
            SetItemContent(item, DBSelectedItemContent, false);

            if (mainLoadout.GetTypeIsEquipped(item.Type))
            {
                ItemDisplay equipped = mainLoadout.GetEquippedFromType(item.Type);
                DBEquipButtonImage.Visibility = equipped.Id == item.Id ? Visibility.Hidden : Visibility.Visible;
                DBUnequipButtonImage.Visibility = equipped.Id == item.Id ? Visibility.Visible : Visibility.Hidden;
                DBEquippedItemName.Text = equipped.Name;
                SetItemContent(equipped, DBEquippedItemContent, false);
            }
            else if (mainLoadout.GetTypeIsEquipped(SelectedBaseItemSocketTarget, SelectedBaseItemSocket))
            {
                ItemDisplay equipped = mainLoadout.GetEquippedFromType(SelectedBaseItemSocketTarget, SelectedBaseItemSocket);
                DBEquipButtonImage.Visibility = equipped.Id == item.Id ? Visibility.Hidden : Visibility.Visible;
                DBUnequipButtonImage.Visibility = equipped.Id == item.Id ? Visibility.Visible : Visibility.Hidden;
                DBEquippedItemName.Text = equipped.Name;
                SetItemContent(equipped, DBEquippedItemContent, false);
            }
            else
            {
                DBEquipButtonImage.Visibility = Visibility.Visible;
                DBUnequipButtonImage.Visibility = Visibility.Hidden;
                DBEquippedItemName.Text = "None";
                DBEquippedItemContent.Children.Clear();
            }
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
                DamageTextBoxes[i].Text = (Output.Damages.GetValueOrDefault((Item.School)i + 1) + Output.Damages.GetValueOrDefault(Item.School.Universal)).ToString(fp);
                FlatDamageTextBoxes[i].Text = (Output.FlatDamages.GetValueOrDefault((Item.School)i + 1) + Output.FlatDamages.GetValueOrDefault(Item.School.Universal)).ToString(fw);
                ResistTextBoxes[i].Text = (Output.Resists.GetValueOrDefault((Item.School)i + 1) + Output.Resists.GetValueOrDefault(Item.School.Universal)).ToString(fp);
                FlatResistanceTextBoxes[i].Text = (Output.FlatResists.GetValueOrDefault((Item.School)i + 1) + Output.FlatResists.GetValueOrDefault(Item.School.Universal)).ToString(fw);
                AccuracyTextBoxes[i].Text = (Output.Accuracies.GetValueOrDefault((Item.School)i + 1) + Output.Accuracies.GetValueOrDefault(Item.School.Universal)).ToString(fp);
                CriticalTextBoxes[i].Text = (Output.Criticals.GetValueOrDefault((Item.School)i + 1) + Output.Criticals.GetValueOrDefault(Item.School.Universal)).ToString(fw);
                CriticalBlockTextBoxes[i].Text = (Output.Blocks.GetValueOrDefault((Item.School)i + 1) + Output.Blocks.GetValueOrDefault(Item.School.Universal)).ToString(fw);
                PierceTextBoxes[i].Text = (Output.Pierces.GetValueOrDefault((Item.School)i + 1) + Output.Pierces.GetValueOrDefault(Item.School.Universal)).ToString(fp);
                PipConversionTextBoxes[i].Text = (Output.PipConversions.GetValueOrDefault((Item.School)i + 1) + Output.PipConversions.GetValueOrDefault(Item.School.Universal)).ToString(fw);
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
            if (ItemDatabaseGrid.SelectedItem is ItemDisplay selectedItem)
            {
                ItemDisplay equipped = mainLoadout.GetEquippedFromType(selectedItem.Type, SelectedBaseItemSocket);
                if (equipped?.Id == selectedItem?.Id)
                {
                    mainLoadout.UnequipItem(selectedItem.Type);
                }
                else
                {
                    mainLoadout.EquipItem(selectedItem, SelectedBaseItemSocket != 0, SelectedBaseItemSocketTarget, SelectedBaseItemSocket);
                    bool wasJewelEquipped = SelectedBaseItemSocket > 0;
                    SelectedBaseItemSocket = 0;
                    if (wasJewelEquipped)
                    {
                        MainTabControl.SelectedIndex = 0;
                        On_StatsTab_BaseItemButton_Click(SelectedBaseItemSocketTarget, null);
                    }
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
                FolderBrowserDialog chooseLocaleDialog = new FolderBrowserDialog()
                {
                    // Place starting folder here...
                };
                if (chooseLocaleDialog.ShowDialog() != FormsDialogResult.OK) return;
                Settings.Default.LocaleFolder = chooseLocaleDialog.SelectedPath;
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

            List<string> files = new List<string>();
            string startingDirectory = "";
            using (var reader = new StreamReader(ofd.FileName))
            {
                startingDirectory = reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    files.Add(startingDirectory + System.IO.Path.DirectorySeparatorChar + reader.ReadLine());
                }
            }
            KiJsonParser<KiTextLocaleBank> kiJsonParser = new KiJsonParser<KiTextLocaleBank>(Settings.Default.LocaleFolder);
            List<KiObject> objects = new List<KiObject>(kiJsonParser.ReadAllToKiObject(files));

            KiSqliteReaderWriter kiSqliteWriter = new KiSqliteReaderWriter();
            kiSqliteWriter.Write(sfd.FileName, objects);
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

            KiSqliteReaderWriter sqliteRW = new KiSqliteReaderWriter();
            ItemViewModel items = this.Resources["itemVM"] as ItemViewModel;
            var newItems = sqliteRW.ReadAllToKiObject(ofd.FileName);

            foreach (var item in newItems.Where(ob => ob is Item i && i.Type >= Item.ItemType.Hat && i.Type <= Item.ItemType.PinSquareSword && !i.Flags.HasFlag(Item.ItemFlags.FLAG_DevItem)))
            {
                items.Add((ItemDisplay)item);
            }
        }
        private void LoadoutButton_Click(object sender, RoutedEventArgs eventArgs)
        {
            LoadoutSelectionModal loadoutSelectionModal = new LoadoutSelectionModal();
            loadoutSelectionModal.ShowDialog();
        }

        MenuItem[] ThemeButtons;
        ResourceDictionary ThemeDictionary
        {
            get
            {
                return Application.Current.Resources.MergedDictionaries[0];
            }
        }

        private void LatteTheme_Click(object sender, RoutedEventArgs eventArgs) => ChangeTheme(sender as MenuItem, "Latte");
        private void FrappeTheme_Click(object sender, RoutedEventArgs eventArgs) => ChangeTheme(sender as MenuItem, "Frappe");
        private void MacchiatoTheme_Click(object sender, RoutedEventArgs eventArgs) => ChangeTheme(sender as MenuItem, "Macchiato");
        private void MochaTheme_Click(object sender, RoutedEventArgs eventArgs) => ChangeTheme(sender as MenuItem, "Mocha");

        private void ChangeTheme(MenuItem theme, string themeName)
        {
            if (theme == null)
            {
                return;
            }

            // Make this submenu act like a radiobox selector
            foreach (MenuItem item in ThemeButtons)
            {
                if (item != theme)
                {
                    item.IsChecked = false;
                    item.IsEnabled = true;
                }
            }
            theme.IsChecked = true;
            theme.IsEnabled = false;

            // Actually change the theme
            ThemeDictionary.MergedDictionaries.Clear();
            Uri themeLocation = new Uri(@$"/Assets/Themes/{themeName}.xaml", UriKind.Relative);
            ThemeDictionary.MergedDictionaries.Add(new ResourceDictionary() { Source = themeLocation });
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
        public Item backingItem { get; protected set; }
        public string Name { get => backingItem.Name; set => backingItem.Name = value; }
        public Guid Id { get => backingItem.Id; set => backingItem.Id = value; }
        public Guid KI_ID { get => backingItem.KiId; set => backingItem.KiId = value; }
        public Guid KI_SetBonusID { get => backingItem.KiSetBonusID; set => backingItem.KiSetBonusID = value; }
        public Item.ItemType Type { get => backingItem.Type; set => backingItem.Type = value; }
        public int LevelRequirement { get => backingItem.LevelRequirement; set => backingItem.LevelRequirement = value; }
        public Item.ItemFlags Flags { get => backingItem.Flags; set => backingItem.Flags = value; }
        public Item.ArenaRank PVPRankRequirement { get => backingItem.PvpRankRequirement; set => backingItem.PvpRankRequirement = value; }
        public Item.ArenaRank PetRankRequirement { get => backingItem.PetRankRequirement; set => backingItem.PetRankRequirement = value; }
        public Item.School SchoolRequirement { get => backingItem.SchoolRequirement; set => backingItem.SchoolRequirement = value; }
        public Item.School SchoolRestriction { get => backingItem.SchoolRestriction; set => backingItem.SchoolRestriction = value; }
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
        public Item.School AltSchoolMastery { get => backingItem.AltSchoolMastery; set => backingItem.AltSchoolMastery = value; }
        public int TearJewelSlots { get => backingItem.TearJewelSlots; set => backingItem.TearJewelSlots = value; }
        public int CircleJewelSlots { get => backingItem.CircleJewelSlots; set => backingItem.CircleJewelSlots = value; }
        public int SquareJewelSlots { get => backingItem.SquareJewelSlots; set => backingItem.SquareJewelSlots = value; }
        public int TriangleJewelSlots { get => backingItem.TriangleJewelSlots; set => backingItem.TriangleJewelSlots = value; }
        public int PowerPinSlots { get => backingItem.PowerPinSlots; set => backingItem.PowerPinSlots = value; }
        public int ShieldPinSlots { get => backingItem.ShieldPinSlots; set => backingItem.ShieldPinSlots = value; }
        public int SwordPinSlots { get => backingItem.SwordPinSlots; set => backingItem.SwordPinSlots = value; }
        public int SetBonusLevel { get => backingItem.SetBonusLevel; set => backingItem.SetBonusLevel = value; }
        public ItemSetBonus SetBonus { get => backingItem.SetBonus; set => backingItem.SetBonus = value; }
        public Dictionary<Item.School, int> Accuracies { get => backingItem.Accuracies; set => backingItem.Accuracies = value; }
        public Dictionary<Item.School, int> Damages { get => backingItem.Damages; set => backingItem.Damages = value; }
        public Dictionary<Item.School, int> Resists { get => backingItem.Resists; set => backingItem.Resists = value; }
        public Dictionary<Item.School, int> Criticals { get => backingItem.Criticals; set => backingItem.Criticals = value; }
        public Dictionary<Item.School, int> Blocks { get => backingItem.Blocks; set => backingItem.Blocks = value; }
        public Dictionary<Item.School, int> Pierces { get => backingItem.Pierces; set => backingItem.Pierces = value; }
        public Dictionary<Item.School, int> FlatDamages { get => backingItem.FlatDamages; set => backingItem.FlatDamages = value; }
        public Dictionary<Item.School, int> FlatResists { get => backingItem.FlatResists; set => backingItem.FlatResists = value; }
        public Dictionary<Item.School, int> PipConversions { get => backingItem.PipConversions; set => backingItem.PipConversions = value; }
        public Dictionary<string, int> ItemCards { get => backingItem.ItemCards; set => backingItem.ItemCards = value; }
        public string DisplayType => Type switch
        {
            Item.ItemType.Shoes => "Boots",
            Item.ItemType.Weapon => "Wand",
            Item.ItemType.TearJewel => "Tear Jewel",
            Item.ItemType.CircleJewel => "Circle Jewel",
            Item.ItemType.SquareJewel => "Square Jewel",
            Item.ItemType.TriangleJewel => "Triangle Jewel",
            Item.ItemType.PinSquarePower => "Power Pin",
            Item.ItemType.PinSquareShield => "Shield Pin",
            Item.ItemType.PinSquareSword => "Sword Pin",
            _ => Type.ToString()
        };
        public string DisplayTypeSource => Type switch
        {
            Item.ItemType.Shoes => "pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Equipment_Boots.png",
            Item.ItemType.Weapon => "pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Equipment_Wand.png",
            Item.ItemType.PinSquarePower => "pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Equipment_PinSquarePower.png",
            Item.ItemType.None => "pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Help.png",
            Item.ItemType.ItemSetBonusData => "pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Help.png",
            _ => "pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Equipment_" + Type.ToString() + ".png"
        };

        public string DisplaySchool => SchoolRequirement switch
        {
            Item.School.Universal => SchoolRestriction != Item.School.Universal ?
                "Not " + SchoolRestriction.ToString() :
                "Any",
            Item.School.None => "None",
            _ => SchoolRequirement.ToString()
        };

        public string DisplaySchoolSource => SchoolRequirement switch
        {
            Item.School.Universal => SchoolRestriction != Item.School.Universal ?
                "pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_School_Not_" + SchoolRestriction.ToString() + ".png" :
                "pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_School_Global.png",
            Item.School.None => "pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Help.png",
            _ => "pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_School_" + SchoolRequirement.ToString() + ".png"
        };
        public bool IsCrownsOnly => Flags.HasFlag(Item.ItemFlags.FLAG_CrownsOnly);
        public Visibility IsCrownsOnlyVisible => IsCrownsOnly ? Visibility.Visible : Visibility.Hidden;
        public bool IsNoAuction => Flags.HasFlag(Item.ItemFlags.FLAG_NoAuction);
        public Visibility IsNoAuctionOnlyVisible => IsNoAuction ? Visibility.Visible : Visibility.Hidden;
        public bool IsNoTrade => Flags.HasFlag(Item.ItemFlags.FLAG_NoTrade);
        public Visibility IsNoTradeVisible => IsNoTrade ? Visibility.Visible : Visibility.Hidden;
        public bool IsRetired => Flags.HasFlag(Item.ItemFlags.FLAG_Retired);
        public Visibility IsRetiredVisible => IsRetired ? Visibility.Visible : Visibility.Hidden;
        public bool IsCrafted => Flags.HasFlag(Item.ItemFlags.FLAG_Crafted);
        public Visibility IsCraftedVisible => IsCrafted ? Visibility.Visible : Visibility.Hidden;
        public bool IsPVPOnly => Flags.HasFlag(Item.ItemFlags.FLAG_PVPOnly);
        public Visibility IsPVPOnlyVisible => IsPVPOnly ? Visibility.Visible : Visibility.Hidden;
        public bool IsNoPVP => Flags.HasFlag(Item.ItemFlags.FLAG_NoPVP);
        public Visibility IsNoPVPVisible => IsNoPVP ? Visibility.Visible : Visibility.Hidden;
        public bool IsJewel => Type >= Item.ItemType.TearJewel && Type <= Item.ItemType.SwordPin;
        public bool IsDevItem => Flags.HasFlag(Item.ItemFlags.FLAG_DevItem);
        public int GetJewelSlots(ItemType type) => type switch
        {
            ItemType.TearJewel => TearJewelSlots,
            ItemType.CircleJewel => CircleJewelSlots,
            ItemType.SquareJewel => SquareJewelSlots,
            ItemType.TriangleJewel => TriangleJewelSlots,
            ItemType.PinSquarePip => PowerPinSlots,
            ItemType.PinSquareShield => ShieldPinSlots,
            ItemType.PinSquareSword => SwordPinSlots,
            _ => 0
        };

        public static Dictionary<string, BitmapImage> StatImages = new Dictionary<string, BitmapImage>()
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
            { "PinSquarePip",      new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Equipment_PinSquarePower.png", UriKind.Absolute)) },
            { "PinSquareShield",      new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Equipment_PinSquareShield.png", UriKind.Absolute)) },
            { "PinSquareSword",      new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Equipment_PinSquareSword.png", UriKind.Absolute)) },
            { "TearJewelFilled",      new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Equipment_TearJewel_Filled.png", UriKind.Absolute)) },
            { "CircleJewelFilled",      new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Equipment_CircleJewel_Filled.png", UriKind.Absolute)) },
            { "SquareJewelFilled",      new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Equipment_SquareJewel_Filled.png", UriKind.Absolute)) },
            { "TriangleJewelFilled",      new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Equipment_TriangleJewel_Filled.png", UriKind.Absolute)) },
            { "PinSquarePipFilled",      new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Equipment_PinSquarePower_Filled.png", UriKind.Absolute)) },
            { "PinSquareShieldFilled",      new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Equipment_PinSquareShield_Filled.png", UriKind.Absolute)) },
            { "PinSquareSwordFilled",      new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Equipment_PinSquareSword_Filled.png", UriKind.Absolute)) },
            { "SpeedBonus",      new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_SpeedBonus.png", UriKind.Absolute)) },
            { "CrownsOnly",      new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Flag_CrownsOnly.png", UriKind.Absolute)) },
            { "NoAuction",      new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Flag_NoAuction.png", UriKind.Absolute)) },
            { "NoHatchmaking",      new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Flag_NoHatchmaking.png", UriKind.Absolute)) },
            { "NoPVP",      new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Flag_NoPVP.png", UriKind.Absolute)) },
            { "PVPOnly",      new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Flag_PVPOnly.png", UriKind.Absolute)) },
            { "NoTrade",      new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Flag_NoTrade.png", UriKind.Absolute)) },
        };

        public TextBlock GetStatDisplay(ItemLoadout loadout, bool showName, bool showIDs)
        {
            void AddSingle(TextBlock tb, bool boldedText = false, params object[] textOrBitmapImage)
            {
                foreach (var item in textOrBitmapImage)
                {
                    if (item is string s)
                    {
                        Run run = new Run(s);
                        if (boldedText) run.FontWeight = FontWeights.Bold;
                        tb.Inlines.Add(run);
                    }
                    else if (item is BitmapImage bi)
                    {
                        Image img = new Image();
                        img.Source = bi;
                        img.Width = 15;
                        img.Height = 15;
                        InlineUIContainer iuc = new InlineUIContainer(img);
                        iuc.BaselineAlignment = BaselineAlignment.Center;
                        tb.Inlines.Add(iuc);
                    }
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

            void AddStats(TextBlock tb, ItemDisplay item)
            {
                if (item == null) return;
                if (item.MaxHealth > 0) AddSingle(tb, false, $"+{item.MaxHealth} Max ", StatImages["Health"], "\n");
                if (item.MaxMana > 0) AddSingle(tb, false, $"+{item.MaxMana} Max ", StatImages["Mana"], "\n");
                if (item.MaxEnergy > 0) AddSingle(tb, false, $"+{item.MaxEnergy} Max ", StatImages["Energy"], "\n");
                if (item.PowerpipChance > 0) AddSingle(tb, false, $"+{item.PowerpipChance}% ", StatImages["PowerPip"], " Chance\n");

                if (item.FishingLuck > 0) AddSingle(tb, false, $"+{item.FishingLuck}% ", StatImages["FishingLuck"], "\n");

                foreach (var pair in item.Accuracies)
                {
                    AddSingle(tb, false, $"+{pair.Value}% ", StatImages[pair.Key.ToString()], StatImages["Accuracy"], "\n");
                }
                foreach (var pair in item.Criticals)
                {
                    AddSingle(tb, false, $"+{pair.Value} ", StatImages[pair.Key.ToString()], StatImages["Critical"], " Rating\n");
                }
                foreach (var pair in item.Blocks)
                {
                    AddSingle(tb, false, $"+{pair.Value} ", StatImages[pair.Key.ToString()], StatImages["Block"], " Rating\n");
                }
                foreach (var pair in item.Damages)
                {
                    AddSingle(tb, false, $"+{pair.Value} ", StatImages[pair.Key.ToString()], StatImages["Damage"], "\n");
                }
                foreach (var pair in item.FlatDamages)
                {
                    AddSingle(tb, false, $"+{pair.Value} ", StatImages[pair.Key.ToString()], StatImages["FlatDamage"], "\n");
                }
                foreach (var pair in item.Resists)
                {
                    AddSingle(tb, false, $"+{pair.Value} ", StatImages[pair.Key.ToString()], StatImages["Resistance"], "\n");
                }
                foreach (var pair in item.FlatResists)
                {
                    AddSingle(tb, false, $"+{pair.Value} ", StatImages[pair.Key.ToString()], StatImages["FlatResistance"], "\n");
                }
                foreach (var pair in item.Pierces)
                {
                    AddSingle(tb, false, $"+{pair.Value} ", StatImages[pair.Key.ToString()], StatImages["ArmorPiercing"], "\n");
                }
                foreach (var pair in item.PipConversions)
                {
                    AddSingle(tb, false, $"+{pair.Value} ", StatImages[pair.Key.ToString()], StatImages["PipConversion"], " Rating\n");
                }

                if (item.OutgoingHealing > 0) AddSingle(tb, false, $"+{item.OutgoingHealing}% ", StatImages["Outgoing"], StatImages["Healing"], "\n");
                if (item.IncomingHealing > 0) AddSingle(tb, false, $"+{item.IncomingHealing}% ", StatImages["Incoming"], StatImages["Healing"], "\n");

                if (item.PowerpipsGiven > 0) AddSingle(tb, false, $"+{item.PowerpipsGiven} ", StatImages["PowerPip"], "\n");
                if (item.PipsGiven > 0) AddSingle(tb, false, $"+{item.PipsGiven} ", StatImages["Pip"], "\n");
                if (item.ShadowpipRating > 0) AddSingle(tb, false, $"+{item.ShadowpipRating} ", StatImages["Shadowpip"], " Rating\n");
                if (item.StunResistChance > 0) AddSingle(tb, false, $"+{item.StunResistChance}% ", StatImages["StunResistance"], "\n");
                if (item.ArchmasteryRating > 0) AddSingle(tb, false, $"+{item.ArchmasteryRating} ", StatImages["Archmastery"], " Rating\n");

                if (item.SpeedBonus > 0) AddSingle(tb, false, $"+{item.SpeedBonus}% ", StatImages["SpeedBonus"], "\n");

                // TODO: Add spells.
            }

            TextBlock tb = new TextBlock()
            {
                Margin = new Thickness(3, 3, 3, 0)
            };

            if (showName)
            {
                AddSingle(tb, true, Name + "\n");
            }

            if (showIDs)
            {
                AddSingle(tb, false, $"ID: {Id.ToString().ToUpper()}\n");
            }

            if (SetBonus != null)
            {
                AddSingle(tb, false, $"({SetBonus.SetName})\n");
                if (showIDs)
                {
                    AddSingle(tb, false, $"Set Bonus ID: {SetBonus.Id.ToString().ToUpper()}\n");
                }
            }

            AddStats(tb, this);

            if (TotalSlots > 0)
            {
                AddSingle(tb, true, "\nSockets\n");
                (int jewelslots, string imageId, string displayName)[] data =
                {
                    (TearJewelSlots, "TearJewel", "(Tear)"),
                    (CircleJewelSlots, "CircleJewel", "(Circle)"),
                    (SquareJewelSlots, "SquareJewel", "(Square)"),
                    (TriangleJewelSlots, "TriangleJewel", "(Triangle)"),
                    (PowerPinSlots, "PinSquarePip", "(Power)"),
                    (ShieldPinSlots, "PinSquareShield", "(Shield)"),
                    (SwordPinSlots, "PinSquareSword", "(Sword)"),
                };
                int i = 0;
                int totalSlots = 0;
                foreach (var entry in data)
                {
                    totalSlots += entry.jewelslots;
                    for (; i < totalSlots; i++)
                    {
                        ItemDisplay jewel = loadout.GetEquippedFromType(Type, i + 1);
                        if (jewel != null)
                        {
                            AddSingle(tb, false, StatImages[entry.imageId + "Filled"], $" {entry.displayName}\n");
                            AddStats(tb, jewel);
                        }
                        else
                        {
                            AddSingle(tb, false, StatImages[entry.imageId], $" {entry.displayName}\n");
                        }
                    }
                }
                AddSingle(tb, false, "\n");
            }

            if (SchoolRequirement != Item.School.Any) AddSingle(tb, false, StatImages[SchoolRequirement.ToString()], " School Only\n");
            if (LevelRequirement > 1) AddSingle(tb, false, $"Level {LevelRequirement}+ Only\n");

            List<BitmapImage> icons = new List<BitmapImage>();
            if (IsCrownsOnly) icons.Add(StatImages["CrownsOnly"]);
            if (IsNoAuction) icons.Add(StatImages["NoAuction"]);
            if (IsNoTrade) icons.Add(StatImages["NoTrade"]);
            //if (false) icons.Add(StatImages["NoHatchmaking"]);
            if (IsNoPVP) icons.Add(StatImages["NoPVP"]);
            if (IsPVPOnly) icons.Add(StatImages["PVPOnly"]);
            AddIcons(tb, icons);

            if (IsRetired) AddSingle(tb, false, "RETIRED ITEM\n");

            // Item Set Bonuses
            if (SetBonus != null)
            {
                AddSingle(tb, true, $"{SetBonus.SetName} ()\n");
                if (showIDs) AddSingle(tb, false, $"\nSet ID: {SetBonus.Id.ToString().ToUpper()}\n");
                foreach (var item in SetBonus.Bonuses.OrderBy(i => i.SetBonusLevel))
                {
                    AddSingle(tb, loadout.GetSetBonusLevel(SetBonus) >= item.SetBonusLevel, $"{item.SetBonusLevel} Items\n");
                    if (showIDs) AddSingle(tb, false, $"Tier ID: {item.Id.ToString().ToUpper()}\n");
                    AddStats(tb, new ItemDisplay(item));
                }
            }

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
                SchoolRequirement = (Item.School)rand.Next((int)Item.School.Any, (int)Item.School.Shadow),
                PowerpipChance = rand.Next(0, 2) == 1 ? rand.Next(1, 22) : 1,
                ShadowpipRating = rand.Next(0, 2) == 1 ? rand.Next(1, 21) : 1,
                StunResistChance = rand.Next(0, 101),
                Type = (Item.ItemType)rand.Next((int)Item.ItemType.Hat, (int)Item.ItemType.ItemSetBonusData),
                TearJewelSlots = rand.Next(0, 3),
                CircleJewelSlots = rand.Next(0, 3),
                SquareJewelSlots = rand.Next(0, 3),
                TriangleJewelSlots = rand.Next(0, 3),
                PowerPinSlots = rand.Next(0, 3),
                ShieldPinSlots = rand.Next(0, 3),
                SwordPinSlots = rand.Next(0, 3),
            };

            item.SchoolRestriction = item.SchoolRequirement != Item.School.Any ? Item.School.Any : (Item.School)rand.Next((int)Item.School.Any, (int)Item.School.Shadow);
            item.Accuracies.Add(rand.Next(0, 2) == 0 ? Item.School.Any : item.SchoolRequirement, rand.Next(1, 20));
            item.Blocks.Add(rand.Next(0, 2) == 0 ? Item.School.Any : item.SchoolRequirement, rand.Next(1, 20));
            item.Damages.Add(rand.Next(0, 2) == 0 ? Item.School.Any : item.SchoolRequirement, rand.Next(1, 20));
            item.Criticals.Add(rand.Next(0, 2) == 0 ? Item.School.Any : item.SchoolRequirement, rand.Next(1, 20));
            item.FlatDamages.Add(rand.Next(0, 2) == 0 ? Item.School.Any : item.SchoolRequirement, rand.Next(1, 20));
            item.FlatResists.Add(rand.Next(0, 2) == 0 ? Item.School.Any : item.SchoolRequirement, rand.Next(1, 20));
            item.Pierces.Add(rand.Next(0, 2) == 0 ? Item.School.Any : item.SchoolRequirement, rand.Next(1, 20));
            item.PipConversions.Add(rand.Next(0, 2) == 0 ? Item.School.Any : item.SchoolRequirement, rand.Next(1, 20));
            item.Resists.Add(rand.Next(0, 2) == 0 ? Item.School.Any : item.SchoolRequirement, rand.Next(1, 20));


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
        protected Dictionary<ItemSetBonus, int> EquippedBonusLevels;
        protected Dictionary<(ItemType, int), ItemDisplay> EquippedJewels;
        public string Name { get; set; }
        public Item.School WizardSchool { get; set; }
        public string DisplayWizardSchool => WizardSchool switch
        {
            Item.School.Universal => "Any",
            Item.School.None => "None",
            _ => WizardSchool.ToString()
        };

        public string DisplayWizardSchoolSource => WizardSchool switch
        {
            Item.School.Universal => "pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_School_Global.png",
            Item.School.None => "pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Help.png",
            _ => "pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_School_" + WizardSchool.ToString() + ".png"
        };
        public int WizardLevel { get; set; }

        public ItemDisplay CustomStats = null;
        public bool UseCustomStats = false;

        private static Dictionary<FileLevelStats, int[]> ConstantStatValues;
        public ItemLoadout()
        {
            EquippedItems = new List<ItemDisplay>();
            EquippedBonusLevels = new Dictionary<ItemSetBonus, int>();
            EquippedJewels = new Dictionary<(ItemType, int), ItemDisplay>();

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
        private int GetJewelSlots(ItemType equippedItemType, ItemType jewelType)
        {
            return GetEquippedFromType(equippedItemType)?.GetJewelSlots(jewelType) ?? 0;
        }
        public int GetSetBonusLevel(ItemSetBonus set)
        {
            return EquippedItems?.Count(i => i.SetBonus == set) ?? 0;
        }
        public bool GetTypeIsEquipped(ItemType type, int socket = 0)
        {
            if (socket > 0)
                return EquippedJewels.Any(i => i.Key.Item1 == type && i.Key.Item2 == socket);
            else
                return EquippedItems.Any(i => i.Type == type);
        }
        public ItemDisplay GetEquippedFromType(ItemType type, int socket = 0)
        {
            if (socket > 0)
                return EquippedJewels.GetValueOrDefault((type, socket));
            else
                return EquippedItems.FirstOrDefault(i => i.Type == type);
        }
        public void EquipItem(ItemDisplay item, bool IsJewel = false, ItemType attachedItemType = ItemType.None, int socketSlot = 0)
        {
            // If is a jewel
            if (IsJewel)
            {
                EquippedJewels[(attachedItemType, socketSlot)] = item;
            }
            // Any other type
            else
            {
                // If we already have this equipped, remove it.
                if (GetTypeIsEquipped(item.Type))
                {
                    EquippedItems.Remove(GetEquippedFromType(item.Type));
                }

                EquippedItems.Add(item);
            }

            CalculateSetBonuses();
        }
        public void UnequipItem(ItemType type)
        {
            ItemDisplay equipped = EquippedItems.FirstOrDefault(i => i.Type == type);
            if (equipped != null)
            {
                EquippedItems.Remove(equipped);
            }
        }
        public void CalculateSetBonuses()
        {
            EquippedBonusLevels.Clear();
            List<Item> items = new List<Item>();
            foreach (ItemDisplay item in EquippedItems.Where(i => i.Type != ItemType.ItemSetBonusData))
            {
                if (item.SetBonus != null)
                {
                    EquippedBonusLevels.AddOrIncrement(item.SetBonus, 1);
                }
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

            void AddItemStatsToOutput(Item item)
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

            foreach (ItemDisplay item in EquippedItems)
            {
                AddItemStatsToOutput(item.backingItem);
            }

            // Jewel calculation
            foreach (var entry in EquippedJewels)
            {
                AddItemStatsToOutput(entry.Value.backingItem);
            }

            // Set Bonus calculation
            foreach (var entry in EquippedBonusLevels)
            {
                foreach (Item item in entry.Key.Bonuses)
                {
                    if (entry.Value >= item.SetBonusLevel)
                    {
                        AddItemStatsToOutput(item);
                    }
                }
            }

            return Stats;
        }
    }
}