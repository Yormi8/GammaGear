using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using GammaGear.Views;
using Wpf.Ui.Appearance;
using Wpf.Ui.Extensions;

namespace GammaGear
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = new MasterWindow();

            MainWindow.Resources = Resources;

            MainWindow.Show();

            var accent = TryFindResource("CatppuccinAccent");
            if (accent is Color)
            {
                Color accentColor = (Color)accent;
                Color accentAlt1Color = accentColor.UpdateBrightness(-10);
                Color accentAlt2Color = accentColor.UpdateBrightness(-5);
                Color accentAlt3Color = accentColor.UpdateBrightness(-15);

                Accent.Apply(accentColor, accentAlt1Color, accentAlt2Color, accentAlt3Color);
            }

            base.OnStartup(e);
        }
    }
}
