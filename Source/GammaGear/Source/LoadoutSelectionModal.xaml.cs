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
using System.Windows.Shapes;

namespace GammaGear.Source
{
    /// <summary>
    /// Interaction logic for LoadoutSelectionModal.xaml
    /// </summary>
    public partial class LoadoutSelectionModal : Window
    {
        public LoadoutSelectionModal()
        {
            InitializeComponent();
        }
    }

    public class ItemLoadoutViewModel : ObservableCollection<ItemLoadout>
    {
        public ItemLoadoutViewModel() { }
    }
}
