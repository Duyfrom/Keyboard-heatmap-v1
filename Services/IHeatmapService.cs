using System;
using System.Collections.Generic;
using System.Windows.Input;
using KeyboardHeatmapPro.Models;

namespace KeyboardHeatmapPro.Services;

public interface IHeatmapService
{
    event EventHandler<HeatmapUpdateEventArgs>? HeatmapUpdated;
    IEnumerable<KeyInfo> GetKeyboardLayout();
    void UpdateHeatmap(Key key, int sensitivity);
    void ResetHeatmap();
    void SetSensitivity(int level);
    void SetDecayRate(double rate);
    void SetColorScheme(string scheme);
}

public class HeatmapUpdateEventArgs : EventArgs
{
    public IEnumerable<KeyHeatUpdate> Updates { get; }

    public HeatmapUpdateEventArgs(IEnumerable<KeyHeatUpdate> updates)
    {
        Updates = updates;
    }
}

public class KeyHeatUpdate
{
    public Key KeyCode { get; set; }
    public int HeatLevel { get; set; }
}
