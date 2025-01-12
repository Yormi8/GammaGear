using GammaGear.ViewModels;
using GammaGear.ViewModels.Pages;
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
    /// Interaction logic for ItemView.xaml
    /// </summary>
    public partial class ItemView : UserControl
    {
        public ObservableCollection<TextBlock> Stats;

        public ItemView()
        {
            Stats = new ObservableCollection<TextBlock>();

            InitializeComponent();
        }

        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (DataContext is not ItemViewModel ivm)
            {
                return;
            }

            // iterate through ivm stats
            TextBlock tb = new TextBlock();
            tb.Inlines.Add(new Image() { Width = 30, Height = 30, Source = new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Health.png")) });
            tb.Inlines.Add(ivm.MaxHealth.ToString());
            Stats.Add(tb);
            TextBlock tb2 = new TextBlock();
            tb.Inlines.Add(new Image() { Width = 30, Height = 30, Source = new BitmapImage(new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Mana.png")) });
            tb.Inlines.Add(ivm.MaxMana.ToString());
            Stats.Add(tb2);

            StatsPanel.ItemsSource = Stats;
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            StatsPanel.ItemsSource = null;
            Stats.Clear();
        }
    }
}
