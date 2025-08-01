namespace KeyboardHeatmapPro.Models;

public class Settings
{
    public bool AutoStartWithWindows { get; set; }
    public bool StartMinimized { get; set; }
    public bool RememberWindowPosition { get; set; }
    public string GlobalHotkey { get; set; } = "Ctrl+Shift+H";
    public bool ExcludePasswordFields { get; set; } = true;
    public int DataRetentionDays { get; set; } = 30;
    public string Theme { get; set; } = "Light";
    public bool EnableAnimations { get; set; } = true;
    public double AnimationSpeed { get; set; } = 1.0;
}
