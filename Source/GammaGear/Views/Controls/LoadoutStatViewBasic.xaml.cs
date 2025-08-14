using GammaGear.ViewModels.Controls;
using GammaGear.ViewModels.Pages;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for LoadoutStatViewBasic.xaml
    /// </summary>
    public partial class LoadoutStatViewBasic : UserControl
    {
        public ItemLoadoutViewModel ViewModel { get; }

        public LoadoutStatViewBasic()
        {
            InitializeComponent();
        }
    }
}
