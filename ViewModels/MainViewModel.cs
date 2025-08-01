using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KeyboardHeatmapPro.Services;

namespace KeyboardHeatmapPro.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly IKeyboardHookService _keyboardHookService;
    private readonly ISettingsService _settingsService;
    
    [ObservableProperty]
    private ViewModelBase? _currentViewModel;
    
    [ObservableProperty]
    private bool _isTracking;
    
    [ObservableProperty]
    private string _trackingStatus = "Stopped";
    
    [ObservableProperty]
    private int _selectedTabIndex;

    public LiveHeatmapViewModel LiveHeatmapViewModel { get; }
    public AnalyticsViewModel AnalyticsViewModel { get; }
    public TimelineViewModel TimelineViewModel { get; }
    public SettingsViewModel SettingsViewModel { get; }
    public ReportsViewModel ReportsViewModel { get; }

    public MainViewModel(
        IKeyboardHookService keyboardHookService,
        ISettingsService settingsService,
        LiveHeatmapViewModel liveHeatmapViewModel,
        AnalyticsViewModel analyticsViewModel,
        TimelineViewModel timelineViewModel,
        SettingsViewModel settingsViewModel,
        ReportsViewModel reportsViewModel)
    {
        _keyboardHookService = keyboardHookService;
        _settingsService = settingsService;
        
        LiveHeatmapViewModel = liveHeatmapViewModel;
        AnalyticsViewModel = analyticsViewModel;
        TimelineViewModel = timelineViewModel;
        SettingsViewModel = settingsViewModel;
        ReportsViewModel = reportsViewModel;
        
        // Set initial view
        CurrentViewModel = LiveHeatmapViewModel;
        Title = "KeyboardHeatmap Pro";
    }

    partial void OnSelectedTabIndexChanged(int value)
    {
        CurrentViewModel = value switch
        {
            0 => LiveHeatmapViewModel,
            1 => AnalyticsViewModel,
            2 => TimelineViewModel,
            3 => SettingsViewModel,
            4 => ReportsViewModel,
            _ => LiveHeatmapViewModel
        };
    }

    [RelayCommand]
    private void ToggleTracking()
    {
        if (IsTracking)
        {
            StopTracking();
        }
        else
        {
            StartTracking();
        }
    }

    [RelayCommand]
    private void StartTracking()
    {
        try
        {
            _keyboardHookService.StartTracking();
            IsTracking = true;
            TrackingStatus = "Recording";
        }
        catch (Exception ex)
        {
            // Handle error
            TrackingStatus = $"Error: {ex.Message}";
        }
    }

    [RelayCommand]
    private void StopTracking()
    {
        _keyboardHookService.StopTracking();
        IsTracking = false;
        TrackingStatus = "Stopped";
    }

    [RelayCommand]
    private void ShowSettings()
    {
        SelectedTabIndex = 3;
    }

    [RelayCommand]
    private void ShowAbout()
    {
        // TODO: Show about dialog
    }
}
