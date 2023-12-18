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
using Wpf.Ui.Controls;

namespace GammaGear.Views.Pages
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class AboutPage : Page, INavigableView<AboutPageViewModel>
    {
        public AboutPageViewModel ViewModel { get; }

        public AboutPage(AboutPageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }

        private static void NavigateWebLink(Uri uri)
        {
            System.Diagnostics.Process.Start(
                new System.Diagnostics.ProcessStartInfo(uri.AbsoluteUri)
                {
                    UseShellExecute = true
                }
            );
        }

        private void SourceCodeCard_Click(object sender, RoutedEventArgs e)
        {
            NavigateWebLink(new Uri("https://www.github.com/Yormi8/GammaGear"));
        }

        private void LicenseCard_Click(object sender, RoutedEventArgs e)
        {
            NavigateWebLink(new Uri("https://github.com/Yormi8/GammaGear/blob/master/LICENSE.txt"));
        }

        private void FeedbackCard_Click(object sender, RoutedEventArgs e)
        {
            NavigateWebLink(new Uri("https://github.com/Yormi8/GammaGear/issues/new"));
        }
    }
}
