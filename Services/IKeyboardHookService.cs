using System;
using System.Windows.Input;

namespace KeyboardHeatmapPro.Services;

public interface IKeyboardHookService
{
    event EventHandler<KeyPressedEventArgs>? KeyPressed;
    void StartTracking();
    void StopTracking();
    bool IsTracking { get; }
}

public class KeyPressedEventArgs : EventArgs
{
    public Key KeyCode { get; }
    public DateTime Timestamp { get; }
    public string ApplicationName { get; }

    public KeyPressedEventArgs(Key keyCode, DateTime timestamp, string applicationName)
    {
        KeyCode = keyCode;
        Timestamp = timestamp;
        ApplicationName = applicationName;
    }
}
