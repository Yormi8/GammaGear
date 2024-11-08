using GammaGear.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for Debug.xaml
    /// </summary>
    public partial class DebugPage : Page
    {
        public DebugPageViewModel ViewModel { get; }

        public DebugPage(DebugPageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            viewModel.PropertyChanged += OnLogTextUpdated;

            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.LogText += "Line added\n";
        }

        private void OnLogTextUpdated(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "LogText")
            {
                LogScrollViewer.ScrollToEnd();
            }
        }
    }
}
