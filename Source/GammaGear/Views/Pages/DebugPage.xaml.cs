using GammaGear.ViewModels.Pages;
using Microsoft.Extensions.Logging;
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

        private ILogger _logger;

        public DebugPage(DebugPageViewModel viewModel, ILogger<DebugPage> logger)
        {
            ViewModel = viewModel;
            _logger = logger;
            DataContext = this;
            viewModel.PropertyChanged += OnLogTextUpdated;

            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _logger.LogError("Example Error Log");
            _logger.LogWarning("Example Warning Log");
            _logger.LogDebug("Example Debug Log");
            ViewModel.UpdateLogText();
        }

        private void OnLogTextUpdated(object sender, PropertyChangedEventArgs e)
        {
            if (LogTextAutoScrollToggle.IsChecked == true && e.PropertyName == "LogText")
            {
                LogScrollViewer.ScrollToEnd();
            }
        }
    }
}
