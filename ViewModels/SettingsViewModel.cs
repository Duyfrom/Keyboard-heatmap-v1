using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KeyboardHeatmapPro.Models;
using KeyboardHeatmapPro.Services;

namespace KeyboardHeatmapPro.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    private readonly ISettingsService _settingsService;

    [ObservableProperty]
    private bool _autoStartWithWindows;

    [ObservableProperty]
    private bool _startMinimized;

    [ObservableProperty]
    private bool _rememberWindowPosition;

    [ObservableProperty]
    private string _globalHotkey = "Ctrl+Shift+H";

    [ObservableProperty]
    private bool _excludePasswordFields = true;

    [ObservableProperty]
    private int _dataRetentionDays = 30;

    [ObservableProperty]
    private string _selectedTheme = "Light";

    public string[] Themes { get; } = { "Light", "Dark", "Auto" };

    public SettingsViewModel(ISettingsService settingsService)
    {
        _settingsService = settingsService;
        Title = "Settings";
        LoadSettings();
    }

    private void LoadSettings()
    {
        var settings = _settingsService.GetSettings();
        AutoStartWithWindows = settings.AutoStartWithWindows;
        StartMinimized = settings.StartMinimized;
        RememberWindowPosition = settings.RememberWindowPosition;
        GlobalHotkey = settings.GlobalHotkey;
        ExcludePasswordFields = settings.ExcludePasswordFields;
        DataRetentionDays = settings.DataRetentionDays;
        SelectedTheme = settings.Theme;
    }

    [RelayCommand]
    private void SaveSettings()
    {
        var settings = new Settings
        {
            AutoStartWithWindows = AutoStartWithWindows,
            StartMinimized = StartMinimized,
            RememberWindowPosition = RememberWindowPosition,
            GlobalHotkey = GlobalHotkey,
            ExcludePasswordFields = ExcludePasswordFields,
            DataRetentionDays = DataRetentionDays,
            Theme = SelectedTheme
        };

        _settingsService.SaveSettings(settings);
    }

    [RelayCommand]
    private void ResetSettings()
    {
        _settingsService.ResetToDefaults();
        LoadSettings();
    }
}
