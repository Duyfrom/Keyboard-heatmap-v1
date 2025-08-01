using System;
using System.IO;
using System.Text.Json;
using KeyboardHeatmapPro.Models;

namespace KeyboardHeatmapPro.Services;

public class SettingsService : ISettingsService
{
    private readonly string _settingsPath;
    private Settings _currentSettings = null!;
    
    public SettingsService()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var settingsDir = Path.Combine(appData, "KeyboardHeatmapPro");
        Directory.CreateDirectory(settingsDir);
        _settingsPath = Path.Combine(settingsDir, "settings.json");
        
        LoadSettings();
    }
    
    public Settings GetSettings()
    {
        return _currentSettings;
    }
    
    public void SaveSettings(Settings settings)
    {
        _currentSettings = settings;
        var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_settingsPath, json);
    }
    
    public void ResetToDefaults()
    {
        _currentSettings = new Settings();
        SaveSettings(_currentSettings);
    }
    
    private void LoadSettings()
    {
        if (File.Exists(_settingsPath))
        {
            try
            {
                var json = File.ReadAllText(_settingsPath);
                _currentSettings = JsonSerializer.Deserialize<Settings>(json) ?? new Settings();
            }
            catch
            {
                _currentSettings = new Settings();
            }
        }
        else
        {
            _currentSettings = new Settings();
        }
    }
}
