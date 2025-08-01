using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;
using KeyboardHeatmapPro.Models;

namespace KeyboardHeatmapPro.Services;

public class HeatmapService : IHeatmapService
{
    private readonly Dictionary<Key, double> _heatValues = new();
    private readonly DispatcherTimer _decayTimer;
    private double _sensitivity = 5.0;
    private double _decayRate = 1.0;
    private string _colorScheme = "Default";
    
    public event EventHandler<HeatmapUpdateEventArgs>? HeatmapUpdated;
    
    public HeatmapService()
    {
        _decayTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(100)
        };
        _decayTimer.Tick += OnDecayTick;
        _decayTimer.Start();
    }
    
    public IEnumerable<KeyInfo> GetKeyboardLayout()
    {
        // Create standard US QWERTY 104-key layout
        var keys = new List<KeyInfo>();
        
        // Function keys row
        keys.Add(new KeyInfo { KeyCode = Key.Escape, Label = "Esc", X = 0, Y = 0, Width = 40, Height = 40 });
        
        // F1-F12
        for (int i = 0; i < 12; i++)
        {
            keys.Add(new KeyInfo
            {
                KeyCode = Key.F1 + i,
                Label = $"F{i + 1}",
                X = 80 + (i * 45),
                Y = 0,
                Width = 40,
                Height = 40,
                Category = KeyCategory.Function
            });
        }
        
        // Number row
        keys.Add(new KeyInfo { KeyCode = Key.OemTilde, Label = "~\n`", X = 0, Y = 60, Width = 40, Height = 40 });
        for (int i = 0; i < 10; i++)
        {
            var key = Key.D1 + i;
            if (i == 9) key = Key.D0;
            keys.Add(new KeyInfo
            {
                KeyCode = key,
                Label = i == 9 ? "0" : (i + 1).ToString(),
                X = 45 + (i * 45),
                Y = 60,
                Width = 40,
                Height = 40,
                Category = KeyCategory.Number
            });
        }
        keys.Add(new KeyInfo { KeyCode = Key.OemMinus, Label = "-", X = 495, Y = 60, Width = 40, Height = 40 });
        keys.Add(new KeyInfo { KeyCode = Key.OemPlus, Label = "+", X = 540, Y = 60, Width = 40, Height = 40 });
        keys.Add(new KeyInfo { KeyCode = Key.Back, Label = "Backspace", X = 585, Y = 60, Width = 85, Height = 40 });
        
        // QWERTY row
        keys.Add(new KeyInfo { KeyCode = Key.Tab, Label = "Tab", X = 0, Y = 105, Width = 60, Height = 40 });
        string qwertyRow = "QWERTYUIOP";
        for (int i = 0; i < qwertyRow.Length; i++)
        {
            keys.Add(new KeyInfo
            {
                KeyCode = Enum.Parse<Key>(qwertyRow[i].ToString()),
                Label = qwertyRow[i].ToString(),
                X = 65 + (i * 45),
                Y = 105,
                Width = 40,
                Height = 40,
                Category = KeyCategory.Letter
            });
        }
        
        // ASDF row
        keys.Add(new KeyInfo { KeyCode = Key.CapsLock, Label = "Caps Lock", X = 0, Y = 150, Width = 75, Height = 40 });
        string asdfRow = "ASDFGHJKL";
        for (int i = 0; i < asdfRow.Length; i++)
        {
            keys.Add(new KeyInfo
            {
                KeyCode = Enum.Parse<Key>(asdfRow[i].ToString()),
                Label = asdfRow[i].ToString(),
                X = 80 + (i * 45),
                Y = 150,
                Width = 40,
                Height = 40,
                Category = KeyCategory.Letter
            });
        }
        keys.Add(new KeyInfo { KeyCode = Key.Enter, Label = "Enter", X = 585, Y = 150, Width = 85, Height = 40 });
        
        // ZXCV row
        keys.Add(new KeyInfo { KeyCode = Key.LeftShift, Label = "Shift", X = 0, Y = 195, Width = 95, Height = 40 });
        string zxcvRow = "ZXCVBNM";
        for (int i = 0; i < zxcvRow.Length; i++)
        {
            keys.Add(new KeyInfo
            {
                KeyCode = Enum.Parse<Key>(zxcvRow[i].ToString()),
                Label = zxcvRow[i].ToString(),
                X = 100 + (i * 45),
                Y = 195,
                Width = 40,
                Height = 40,
                Category = KeyCategory.Letter
            });
        }
        keys.Add(new KeyInfo { KeyCode = Key.RightShift, Label = "Shift", X = 550, Y = 195, Width = 120, Height = 40 });
        
        // Space bar row
        keys.Add(new KeyInfo { KeyCode = Key.LeftCtrl, Label = "Ctrl", X = 0, Y = 240, Width = 60, Height = 40 });
        keys.Add(new KeyInfo { KeyCode = Key.LWin, Label = "Win", X = 65, Y = 240, Width = 45, Height = 40 });
        keys.Add(new KeyInfo { KeyCode = Key.LeftAlt, Label = "Alt", X = 115, Y = 240, Width = 45, Height = 40 });
        keys.Add(new KeyInfo { KeyCode = Key.Space, Label = "Space", X = 165, Y = 240, Width = 270, Height = 40 });
        keys.Add(new KeyInfo { KeyCode = Key.RightAlt, Label = "Alt", X = 440, Y = 240, Width = 45, Height = 40 });
        keys.Add(new KeyInfo { KeyCode = Key.RightCtrl, Label = "Ctrl", X = 610, Y = 240, Width = 60, Height = 40 });
        
        return keys;
    }
    
    public void UpdateHeatmap(Key key, int sensitivity)
    {
        if (!_heatValues.ContainsKey(key))
            _heatValues[key] = 0;
        
        _heatValues[key] = Math.Min(10, _heatValues[key] + (sensitivity / 10.0));
        
        RaiseHeatmapUpdated();
    }
    
    public void ResetHeatmap()
    {
        _heatValues.Clear();
        RaiseHeatmapUpdated();
    }
    
    public void SetSensitivity(int level)
    {
        _sensitivity = level;
    }
    
    public void SetDecayRate(double rate)
    {
        _decayRate = rate;
    }
    
    public void SetColorScheme(string scheme)
    {
        _colorScheme = scheme;
        RaiseHeatmapUpdated();
    }
    
    private void OnDecayTick(object? sender, EventArgs e)
    {
        bool hasChanges = false;
        var keysToUpdate = _heatValues.Keys.ToList();
        
        foreach (var key in keysToUpdate)
        {
            if (_heatValues[key] > 0)
            {
                _heatValues[key] = Math.Max(0, _heatValues[key] - (_decayRate * 0.01));
                hasChanges = true;
            }
        }
        
        if (hasChanges)
        {
            RaiseHeatmapUpdated();
        }
    }
    
    private void RaiseHeatmapUpdated()
    {
        var updates = _heatValues.Select(kvp => new KeyHeatUpdate
        {
            KeyCode = kvp.Key,
            HeatLevel = (int)Math.Round(kvp.Value)
        });
        
        HeatmapUpdated?.Invoke(this, new HeatmapUpdateEventArgs(updates));
    }
}
