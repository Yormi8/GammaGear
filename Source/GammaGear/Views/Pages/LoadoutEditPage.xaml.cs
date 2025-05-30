using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GammaGear.ViewModels;
using GammaGear.ViewModels.Pages;
using Microsoft.Extensions.Logging;
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
using Wpf.Ui.Abstractions.Controls;

namespace GammaGear.Views.Pages
{
    /// <summary>
    /// Interaction logic for LoadoutEditPage.xaml
    /// </summary>
    public partial class LoadoutEditPage : INavigableView<LoadoutEditViewModel>
    {
        public LoadoutEditViewModel ViewModel { get; }

        private readonly ILogger<LoadoutEditViewModel> _logger;

        public LoadoutEditPage(ILogger<LoadoutEditViewModel> logger, LoadoutEditViewModel viewModel)
        {
            _logger = logger;
            ViewModel = viewModel;
            DataContext = this;
            this.DataContextChanged += DataContextChangedHandler;

            InitializeComponent();
        }

        private void DataContextChangedHandler(object sender, DependencyPropertyChangedEventArgs e)
        {
            _logger.LogDebug("Data Context Changed");
            if (e.NewValue is LoadoutEditViewModel ilvm)
            {
                _logger.LogDebug("Header Value updated");
                ViewModel.Title = ilvm.CurrentLoadout.Name;
            }
        }
    }
}
