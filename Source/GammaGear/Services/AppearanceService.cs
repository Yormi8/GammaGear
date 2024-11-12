using GammaGear.Models;
using GammaGear.Services.Contracts;
using GammaGear.Views;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace GammaGear.Services
{
    public class AppearanceService : IAppearanceService
    {
        private ApplicationTheme _currentTheme = (ApplicationTheme)(-1);
        private bool _isHooked = false;
        private readonly IThemeService _themeService;
        private readonly ILogger _logger;

        public AppearanceService(IThemeService themeService, ILogger<AppearanceService> logger)
        {
            _themeService = themeService;
            _logger = logger;
        }

        private Wpf.Ui.Appearance.ApplicationTheme ToWpfUiTheme(ApplicationTheme theme) =>
            theme switch
            {
                ApplicationTheme.Light => Wpf.Ui.Appearance.ApplicationTheme.Light,
                ApplicationTheme.Dark => Wpf.Ui.Appearance.ApplicationTheme.Dark,
                ApplicationTheme.System => _themeService.GetSystemTheme(),
                _ => Wpf.Ui.Appearance.ApplicationTheme.Unknown,
            };

        public ApplicationTheme GetTheme() => _currentTheme;

        public void SetTheme(ApplicationTheme theme)
        {
            if (theme == _currentTheme || (theme == ApplicationTheme.System && _isHooked))
            {
                return;
            }

            _currentTheme = theme;

            if (theme == ApplicationTheme.System)
            {
                HookToSystemTheme();
                return;
            }

            UnhookFromSystemTheme();
            _themeService.SetTheme(ToWpfUiTheme(theme));

            _logger.LogInformation("Set theme to {theme}", theme);
        }

        private void HookToSystemTheme()
        {
            if (_isHooked)
            {
                return;
            }

            IWindow window = App.GetService<IWindow>();

            if (window is not MainWindow mainWindow)
            {
                return;
            }

            Wpf.Ui.Appearance.SystemThemeWatcher.Watch(
                mainWindow,              // Window class
                WindowBackdropType.Mica, // Background type
                true                     // Whether to change accents automatically);
            );
            _logger.LogInformation("Started to watch system theme");

            Wpf.Ui.Appearance.ApplicationThemeManager.ApplySystemTheme(true);
            _logger.LogInformation("Set theme to system");

            _isHooked = true;

        }

        private void UnhookFromSystemTheme()
        {
            if (!_isHooked)
            {
                return;
            }

            IWindow window = App.GetService<IWindow>();

            if (window is not MainWindow mainWindow)
            {
                return;
            }

            Wpf.Ui.Appearance.SystemThemeWatcher.UnWatch(
                mainWindow              // Window class
            );

            _isHooked = false;

            _logger.LogInformation("No longer watching system theme");
        }
    }
}
