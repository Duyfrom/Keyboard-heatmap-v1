using System;
using System.ComponentModel;
using System.Windows;
using KeyboardHeatmapPro.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace KeyboardHeatmapPro;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly MainViewModel _viewModel;
    
    public MainWindow(MainViewModel viewModel)
    {
        Console.WriteLine("MainWindow constructor called");
        try
        {
            InitializeComponent();
            Console.WriteLine("InitializeComponent completed");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in InitializeComponent: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            throw;
        }
        _viewModel = viewModel;
        DataContext = _viewModel;
        
        // Handle window closing
        Closing += OnClosing;
        
        // Ensure window is visible
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        Console.WriteLine($"Window visibility: {Visibility}");
    }
    
    private void OnClosing(object? sender, CancelEventArgs e)
    {
        // Check if we should minimize to tray instead of closing
        if (_viewModel.SettingsViewModel.StartMinimized)
        {
            e.Cancel = true;
            WindowState = WindowState.Minimized;
            Hide();
        }
    }
    
    protected override void OnStateChanged(EventArgs e)
    {
        base.OnStateChanged(e);
        
        if (WindowState == WindowState.Minimized && _viewModel.SettingsViewModel.StartMinimized)
        {
            Hide();
        }
    }
}
