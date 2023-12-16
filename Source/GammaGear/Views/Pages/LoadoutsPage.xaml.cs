using GammaGear.ViewModels;
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
using Wpf.Ui.Controls;

namespace GammaGear.Views.Pages
{
    /// <summary>
    /// Interaction logic for ItemLoadoutListingView.xaml
    /// </summary>
    public partial class LoadoutsPage : INavigableView<LoadoutsViewModel>
    {
        public LoadoutsViewModel ViewModel { get; }

        public LoadoutsPage(LoadoutsViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }
    }
}
