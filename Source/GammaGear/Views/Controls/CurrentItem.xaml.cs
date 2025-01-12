using CommunityToolkit.Mvvm.DependencyInjection;
using GammaGear.Extensions;
using GammaGear.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace GammaGear.Views.Controls
{
    /// <summary>
    /// Interaction logic for CurrentItem.xaml
    /// </summary>
    public partial class CurrentItem : UserControl
    {
        public CurrentItem()
        {
            InitializeComponent();
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is ItemViewModel item)
            {
                Stats.Children.Clear();
                if (item.MaxHealth > 0)
                    Stats.Children.Add(CreateStat($"+{item.MaxHealth} Max ", GammaExtensions.ToIconUri("health")));
                if (item.MaxMana > 0)
                    Stats.Children.Add(CreateStat($"+{item.MaxMana} Max ", GammaExtensions.ToIconUri("mana")));
                if (item.MaxEnergy > 0)
                    Stats.Children.Add(CreateStat($"+{item.MaxEnergy} Max ", GammaExtensions.ToIconUri("energy")));

                foreach (var s in item.Accuracies)
                    Stats.Children.Add(CreateStat($"+{s.Value} ", s.Key.ToIconUri(), GammaExtensions.ToIconUri("accuracy")));
                foreach (var s in item.Pierces)
                    Stats.Children.Add(CreateStat($"+{s.Value} ", s.Key.ToIconUri(), GammaExtensions.ToIconUri("armorpiercing")));
                foreach (var s in item.Criticals)
                    Stats.Children.Add(CreateStat($"+{s.Value} ", s.Key.ToIconUri(), GammaExtensions.ToIconUri("critical"), " Rating"));
                foreach (var s in item.Blocks)
                    Stats.Children.Add(CreateStat($"+{s.Value} ", s.Key.ToIconUri(), GammaExtensions.ToIconUri("criticalblock"), " Rating"));
                foreach (var s in item.Damages)
                    Stats.Children.Add(CreateStat($"+{s.Value} ", s.Key.ToIconUri(), GammaExtensions.ToIconUri("damage")));
                foreach (var s in item.FlatDamages)
                    Stats.Children.Add(CreateStat($"+{s.Value} ", s.Key.ToIconUri(), GammaExtensions.ToIconUri("flatdamage")));
                foreach (var s in item.FlatResists)
                    Stats.Children.Add(CreateStat($"+{s.Value} ", s.Key.ToIconUri(), GammaExtensions.ToIconUri("flatresistance")));
                foreach (var s in item.Resists)
                    Stats.Children.Add(CreateStat($"+{s.Value} ", s.Key.ToIconUri(), GammaExtensions.ToIconUri("resistance")));
                foreach (var s in item.PipConversions)
                    Stats.Children.Add(CreateStat($"+{s.Value} ", s.Key.ToIconUri(), GammaExtensions.ToIconUri("pipconversion")));

                if (item.IncomingHealing > 0)
                    Stats.Children.Add(CreateStat($"+{item.IncomingHealing}% ", GammaExtensions.ToIconUri("incoming")));
                if (item.OutgoingHealing > 0)
                    Stats.Children.Add(CreateStat($"+{item.OutgoingHealing}% ", GammaExtensions.ToIconUri("outgoing")));

                if (item.StunResistChance > 0)
                    Stats.Children.Add(CreateStat($"+{item.StunResistChance}% ", GammaExtensions.ToIconUri("stunresistance")));

                if (item.ArchmasteryRating > 0)
                    Stats.Children.Add(CreateStat($"+{item.ArchmasteryRating} ", GammaExtensions.ToIconUri("archmastery"), " Rating"));
                if (item.PowerpipChance > 0)
                    Stats.Children.Add(CreateStat($"+{item.PowerpipChance}% ", GammaExtensions.ToIconUri("powerpip"), " Chance"));
                if (item.ShadowpipRating > 0)
                    Stats.Children.Add(CreateStat($"+{item.ShadowpipRating} ", GammaExtensions.ToIconUri("shadowpip"), " Rating"));

                if (item.FishingLuck > 0)
                    Stats.Children.Add(CreateStat($"+{item.FishingLuck}% ", GammaExtensions.ToIconUri("fishingluck")));

                if (item.PipsGiven > 0)
                    Stats.Children.Add(CreateStat($"+{item.PipsGiven} ", GammaExtensions.ToIconUri("pip")));
                if (item.PowerpipsGiven > 0)
                    Stats.Children.Add(CreateStat($"+{item.PowerpipsGiven} ", GammaExtensions.ToIconUri("powerpip")));

                if (item.SpeedBonus > 0)
                    Stats.Children.Add(CreateStat($"+{item.SpeedBonus}% ", GammaExtensions.ToIconUri("speedbonus")));
            }
        }

        private TextBlock CreateStat(params object[] elements)
        {
            TextBlock stat = new TextBlock();
            foreach (object element in elements)
            {
                if (element is string s)
                {
                    stat.Inlines.Add(s);
                }
                else if (element is Uri u)
                {
                    stat.Inlines.Add(new InlineUIContainer(new Image()
                    {
                        Source = new BitmapImage(u),
                        Height = 15,
                        Width = 15,
                    })
                    {
                        BaselineAlignment = BaselineAlignment.Center,
                    });
                }
            }
            return stat;
        }
    }
}
