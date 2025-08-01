using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KeyboardHeatmapPro.Models;
using KeyboardHeatmapPro.Services;

namespace KeyboardHeatmapPro.ViewModels;

public partial class LiveHeatmapViewModel : ViewModelBase
{
    private readonly IKeyboardHookService _keyboardHookService;
    private readonly IHeatmapService _heatmapService;
    private readonly IDataService _dataService;
    
    [ObservableProperty]
    private ObservableCollection<KeyViewModel> _keys = new();
    
    [ObservableProperty]
    private TimeSpan _sessionDuration;
    
    [ObservableProperty]
    private int _totalKeystrokes;
    
    [ObservableProperty]
    private double _keysPerMinute;
    
    [ObservableProperty]
    private double _wordsPerMinute;
    
    [ObservableProperty]
    private int _sensitivityLevel = 5;
    
    [ObservableProperty]
    private double _decayRate = 1.0;
    
    [ObservableProperty]
    private bool _showLetters = true;
    
    [ObservableProperty]
    private bool _showNumbers = true;
    
    [ObservableProperty]
    private bool _showSymbols = true;
    
    [ObservableProperty]
    private bool _showFunctionKeys = true;
    
    [ObservableProperty]
    private string _colorScheme = "Default";

    public ObservableCollection<string> ColorSchemes { get; } = new()
    {
        "Default",
        "Fire",
        "Ocean",
        "Forest",
        "Monochrome"
    };

    public LiveHeatmapViewModel(
        IKeyboardHookService keyboardHookService,
        IHeatmapService heatmapService,
        IDataService dataService)
    {
        _keyboardHookService = keyboardHookService;
        _heatmapService = heatmapService;
        _dataService = dataService;
        
        Title = "Live Heatmap";
        InitializeKeyboard();
        SubscribeToEvents();
    }

    private void InitializeKeyboard()
    {
        // Initialize standard 104-key layout
        var keyboardLayout = _heatmapService.GetKeyboardLayout();
        foreach (var key in keyboardLayout)
        {
            Keys.Add(new KeyViewModel
            {
                KeyCode = key.KeyCode,
                Label = key.Label,
                X = key.X,
                Y = key.Y,
                Width = key.Width,
                Height = key.Height,
                HeatLevel = 0,
                PressCount = 0
            });
        }
    }

    private void SubscribeToEvents()
    {
        _keyboardHookService.KeyPressed += OnKeyPressed;
        _heatmapService.HeatmapUpdated += OnHeatmapUpdated;
    }

    private void OnKeyPressed(object? sender, KeyPressedEventArgs e)
    {
        TotalKeystrokes++;
        
        var key = Keys.FirstOrDefault(k => k.KeyCode == e.KeyCode);
        if (key != null)
        {
            key.PressCount++;
            _heatmapService.UpdateHeatmap(e.KeyCode, SensitivityLevel);
        }
        
        // Update metrics
        UpdateMetrics();
    }

    private void OnHeatmapUpdated(object? sender, HeatmapUpdateEventArgs e)
    {
        foreach (var update in e.Updates)
        {
            var key = Keys.FirstOrDefault(k => k.KeyCode == update.KeyCode);
            if (key != null)
            {
                key.HeatLevel = update.HeatLevel;
            }
        }
    }

    private void UpdateMetrics()
    {
        // Calculate KPM and WPM
        if (SessionDuration.TotalMinutes > 0)
        {
            KeysPerMinute = TotalKeystrokes / SessionDuration.TotalMinutes;
            WordsPerMinute = KeysPerMinute / 5.0; // Assuming average word length of 5 characters
        }
    }

    [RelayCommand]
    private void ResetHeatmap()
    {
        foreach (var key in Keys)
        {
            key.HeatLevel = 0;
            key.PressCount = 0;
        }
        
        TotalKeystrokes = 0;
        SessionDuration = TimeSpan.Zero;
        KeysPerMinute = 0;
        WordsPerMinute = 0;
        
        _heatmapService.ResetHeatmap();
    }

    [RelayCommand]
    private void ExportHeatmap()
    {
        // TODO: Implement heatmap export
    }

    partial void OnSensitivityLevelChanged(int value)
    {
        _heatmapService.SetSensitivity(value);
    }

    partial void OnDecayRateChanged(double value)
    {
        _heatmapService.SetDecayRate(value);
    }

    partial void OnColorSchemeChanged(string value)
    {
        _heatmapService.SetColorScheme(value);
    }
}
