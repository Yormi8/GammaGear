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
using GammaGear.Services.Contracts;
using GammaGear.ViewModels;
using GammaGear.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Wpf.Ui.Appearance;
using Wpf.Ui.Extensions;
using Wpf.Ui;
using Wpf.Ui.Controls;
using GammaGear.ViewModels.Pages;
using NReco.Logging.File;
using System.Runtime.CompilerServices;

namespace GammaGear
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly IHost _host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(config =>
            {
                config.AddJsonFile("AppSettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureLogging((context, logging) =>
            {
                // Unsure what this function is supposed to accomplish? Application Host level logging?
                //var loggingConfig = context.Configuration.GetSection("Logging");
                //logging.AddFile(loggingConfig);
                //logging.AddConsole();
            })
            .ConfigureServices((context, services) =>
            {
                // App Host
                services.AddHostedService<ApplicationHostService>();

                // Logging
                services.AddLogging(loggingBuilder =>
                {
                    var loggingConfig = context.Configuration.GetSection("Logging");
                    loggingBuilder.AddFile(loggingConfig);
                    loggingBuilder.AddConsole();
                });

                // Theme manipulation
                services.AddSingleton<IThemeService, ThemeService>();
                services.AddSingleton<IAppearanceService, AppearanceService>();

                // Persistant user preferences
                services.AddSingleton<UserPreferencesService>();

                // Provides windows service
                services.AddSingleton<WindowsProviderService>();

                // Navigation service without INavigationWindow
                services.AddSingleton<INavigationService, NavigationService>();

                // Provides setup service
                services.AddSingleton<ISetupService, SetupService>();

                // Main window navigation
                services.AddSingleton<IWindow, MainWindow>();
                services.AddSingleton<MainViewModel>();

                // Views and ViewModels
                services.AddSingleton<Views.Pages.HomePage>();
                services.AddSingleton<HomePageViewModel>();

                services.AddSingleton<Views.Pages.LoadoutsPage>();
                services.AddSingleton<LoadoutsPageViewModel>();

                services.AddSingleton<Views.Pages.ItemsPage>();
                services.AddSingleton<ItemsPageViewModel>();

                services.AddSingleton<Views.Pages.PetsPage>();
                services.AddSingleton<PetsPageViewModel>();

                services.AddSingleton<Views.Pages.SettingsPage>();
                services.AddSingleton<SettingsPageViewModel>();

                services.AddSingleton<Views.Pages.DebugPage>();
                services.AddSingleton<DebugPageViewModel>();

                services.AddSingleton<Views.Pages.AboutPage>();
                services.AddSingleton<AboutPageViewModel>();

            })
            .Build();

        public static T GetService<T>() where T : class
        {
            return _host.Services.GetService(typeof(T)) as T;
        }

        protected void OnStartup(object sender, StartupEventArgs e)
        {
            _host.Start();

            // Get the service because it needs to be initialized before it gets selected in the menu.
            var d = _host.Services.GetService<DebugPageViewModel>();
        }

        protected void OnExit(object sender, ExitEventArgs e)
        {
            _host.StopAsync().Wait();

            _host.Dispose();
        }
    }
}
