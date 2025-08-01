using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using KeyboardHeatmapPro.Services;
using KeyboardHeatmapPro.ViewModels;
using KeyboardHeatmapPro.Views;

namespace KeyboardHeatmapPro;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly IHost _host;
    
    public string Version { get; } = "v1.0.0";

    public App()
    {
        // Handle unhandled exceptions
        AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        DispatcherUnhandledException += OnDispatcherUnhandledException;
        
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                ConfigureServices(services);
            })
            .Build();
    }
    
    private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        var ex = e.ExceptionObject as Exception;
        Console.WriteLine($"Unhandled exception: {ex?.Message}");
        Console.WriteLine(ex?.StackTrace);
    }
    
    private void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
    {
        Console.WriteLine($"Dispatcher unhandled exception: {e.Exception.Message}");
        Console.WriteLine(e.Exception.StackTrace);
        e.Handled = true; // Prevent crash
    }

    private void ConfigureServices(IServiceCollection services)
    {
        // Register services
        services.AddSingleton<IKeyboardHookService, KeyboardHookService>();
        services.AddSingleton<IDataService, DataService>();
        services.AddSingleton<ISettingsService, SettingsService>();
        services.AddSingleton<IHeatmapService, HeatmapService>();
        services.AddSingleton<IAnalyticsService, AnalyticsService>();
        services.AddSingleton<IReportService, ReportService>();

        // Register ViewModels
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<LiveHeatmapViewModel>();
        services.AddSingleton<AnalyticsViewModel>();
        services.AddSingleton<TimelineViewModel>();
        services.AddSingleton<SettingsViewModel>();
        services.AddSingleton<ReportsViewModel>();

        // Register Views
        services.AddSingleton<MainWindow>();
        services.AddTransient<LiveHeatmapView>();
        services.AddTransient<AnalyticsView>();
        services.AddTransient<TimelineView>();
        services.AddTransient<SettingsView>();
        services.AddTransient<ReportsView>();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        try
        {
            Console.WriteLine("Starting application...");
            await _host.StartAsync();
            Console.WriteLine("Host started.");

            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            Console.WriteLine("MainWindow created.");
            
            // Set as main window
            MainWindow = mainWindow;
            
            mainWindow.Show();
            Console.WriteLine("MainWindow shown.");
            Console.WriteLine($"MainWindow IsVisible: {mainWindow.IsVisible}");
            Console.WriteLine($"MainWindow WindowState: {mainWindow.WindowState}");

            base.OnStartup(e);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during startup: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            MessageBox.Show($"Error starting application: {ex.Message}", "Startup Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Application.Current.Shutdown();
        }
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        using (_host)
        {
            await _host.StopAsync();
        }
        base.OnExit(e);
    }
}

