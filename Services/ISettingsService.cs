using KeyboardHeatmapPro.Models;

namespace KeyboardHeatmapPro.Services;

public interface ISettingsService
{
    Settings GetSettings();
    void SaveSettings(Settings settings);
    void ResetToDefaults();
}
