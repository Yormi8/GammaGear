using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using GammaGear;
using GammaGear.Services;
using GammaGear.ViewModels;
using GammaGear.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Wpf.Ui.Appearance;
using Wpf.Ui.Extensions;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Mvvm.Services;

namespace GammaGear
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly IHost _host = Host.CreateDefaultBuilder()
            .ConfigureLogging(l =>
            {

            })
            .ConfigureAppConfiguration(config =>
            {

            })
            .ConfigureServices(services =>
            {
                // App Host
                services.AddHostedService<ApplicationHostService>();

                // Theme manipulation
                services.AddSingleton<IThemeService, ThemeService>();

                // Page service
                services.AddSingleton<IPageService, PageService>();

                // Navigation service without INavigationWindow
                services.AddSingleton<INavigationService, NavigationService>();

                // Main window navigation
                services.AddScoped<INavigationWindow, Views.MainWindow>();
                services.AddScoped<MainViewModel>();

                // Views and ViewModels
                services.AddScoped<Views.Pages.Home>();
                services.AddScoped<HomeViewModel>();

                services.AddScoped<Views.Pages.Loadouts>();
                services.AddScoped<LoadoutsViewModel>();

                services.AddScoped<Views.Pages.Items>();
                services.AddScoped<ItemsViewModel>();

                services.AddScoped<Views.Pages.Pets>();
                services.AddScoped<PetsViewModel>();

                services.AddScoped<Views.Pages.Settings>();
                services.AddScoped<SettingsViewModel>();

                services.AddScoped<Views.Pages.Debug>();
                services.AddScoped<DebugViewModel>();

                services.AddScoped<Views.Pages.About>();
                services.AddScoped<AboutViewModel>();

            })
            .Build();

        public static T GetService<T>() where T : class
        {
            return _host.Services.GetService(typeof(T)) as T;
        }

        protected async void OnStartup(object sender, StartupEventArgs e)
        {
            await _host.StartAsync();
        }

        protected async void OnExit(object sender, ExitEventArgs e)
        {
            await _host.StopAsync();

            _host.Dispose();
        }
    }
}
