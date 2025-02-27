using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection;
using System.Windows.Threading;
using UiDesktopApp2.DataAccess;
using UiDesktopApp2.DataAccess.Repositories;
using UiDesktopApp2.Helpers;
using UiDesktopApp2.Services;
using UiDesktopApp2.ViewModels.Pages;
using UiDesktopApp2.ViewModels.Windows;
using UiDesktopApp2.Views.Pages;
using UiDesktopApp2.Views.Windows;
using Wpf.Ui;

namespace UiDesktopApp2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        // The.NET Generic Host provides dependency injection, configuration, logging, and other services.
        // https://docs.microsoft.com/dotnet/core/extensions/generic-host
        // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
        // https://docs.microsoft.com/dotnet/core/extensions/configuration
        // https://docs.microsoft.com/dotnet/core/extensions/logging
        private static readonly IHost _host = Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration(c => { c.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)); })
            .ConfigureServices((context, services) =>
            {
                services.AddHostedService<ApplicationHostService>();
                services.AddDbContext<ApplicationDbContext>();

                // Page resolver service
                services.AddSingleton<IPageService, PageService>();

                // Theme manipulation
                services.AddSingleton<IThemeService, ThemeService>();

                // TaskBar manipulation
                services.AddSingleton<ITaskBarService, TaskBarService>();

                // Service containing navigation, same as INavigationWindow... but without window
                services.AddSingleton<INavigationService, NavigationService>();

                // Dialog service
                services.AddSingleton<IContentDialogService, ContentDialogService>();

                // Automapper
                services.AddSingleton<IMapper>(sp =>
                {
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.AddProfile<MappingProfile>(); // Add your AutoMapper profile
                    });

                    return config.CreateMapper();
                });

                // Global state
                services.AddSingleton<GlobalState>();
                services.AddSingleton<Settings>();

                // Repositories
                services.AddScoped<TestRepository>();
                services.AddScoped<SubjectRepository>();
                services.AddScoped<ResultRepository>();
                services.AddScoped<ImageSetRepository>();

                // Main window with navigation
                services.AddSingleton<INavigationWindow, MainWindow>();
                services.AddSingleton<MainWindowViewModel>();
               
                // Main pages
                services.AddSingleton<DashboardPage>();
                services.AddSingleton<DashboardViewModel>();
                services.AddSingleton<DataPage>();
                services.AddSingleton<DataViewModel>();
                services.AddSingleton<SettingsPage>();
                services.AddSingleton<SettingsViewModel>();
                services.AddSingleton<ConfigurationPage>();
                services.AddSingleton<ConfigurationViewModel>();
                services.AddSingleton<SubjectsPage>();
                services.AddSingleton<SubjectsViewModel>();
                services.AddSingleton<TempPage>();
                services.AddSingleton<TempViewModel>();

                // Sub pages
                services.AddTransient<TestManagePage>();
                services.AddTransient<TestManageViewModel>();
                services.AddTransient<TestRunPage>();
                services.AddTransient<TestRunViewModel>();
                services.AddTransient<TestResultPage>();
                services.AddTransient<TestResultViewModel>();
            }).Build();

        /// <summary>
        /// Gets registered service.
        /// </summary>
        /// <typeparam name="T">Type of the service to get.</typeparam>
        /// <returns>Instance of the service or <see langword="null"/>.</returns>
        public static T GetService<T>()
            where T : class
        {
            return _host.Services.GetService(typeof(T)) as T;
        }

        /// <summary>
        /// Occurs when the application is loading.
        /// </summary>
        private void OnStartup(object sender, StartupEventArgs e)
        {
            _host.Start();
        }

        /// <summary>
        /// Occurs when the application is closing.
        /// </summary>
        private async void OnExit(object sender, ExitEventArgs e)
        {
            await _host.StopAsync();

            _host.Dispose();
        }

        /// <summary>
        /// Occurs when an exception is thrown by an application but not handled.
        /// </summary>
        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // For more info see https://docs.microsoft.com/en-us/dotnet/api/system.windows.application.dispatcherunhandledexception?view=windowsdesktop-6.0
        }
    }
}
