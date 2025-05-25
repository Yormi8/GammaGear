using GammaGear.Services;
using GammaGear.ViewModels.Windows;
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
using System.Windows.Shapes;
using Wpf.Ui.Controls;

namespace GammaGear.Views.Windows
{
    /// <summary>
    /// Interaction logic for SandboxWindow.xaml
    /// </summary>
    public partial class SandboxWindow : FluentWindow
    {
        public SandboxWindowViewModel ViewModel { get; init; }
        private PatchClientService _patchClientService = null;

        public SandboxWindow(SandboxWindowViewModel viewModel, PatchClientService patchClientService)
        {
            ViewModel = viewModel;
            _patchClientService = patchClientService;
            DataContext = this;

            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(_patchClientService.GetLiveRevision);
        }
    }
}
