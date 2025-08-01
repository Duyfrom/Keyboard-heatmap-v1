using System.Windows.Input;

namespace KeyboardHeatmapPro.Models;

public class KeyInfo
{
    public Key KeyCode { get; set; }
    public string Label { get; set; } = string.Empty;
    public double X { get; set; }
    public double Y { get; set; }
    public double Width { get; set; } = 40;
    public double Height { get; set; } = 40;
    public KeyCategory Category { get; set; }
}

public enum KeyCategory
{
    Letter,
    Number,
    Symbol,
    Function,
    Modifier,
    Navigation,
    Special
}
