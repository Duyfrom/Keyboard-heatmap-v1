using System.Windows.Input;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace KeyboardHeatmapPro.ViewModels;

public partial class KeyViewModel : ObservableObject
{
    [ObservableProperty]
    private Key _keyCode;

    [ObservableProperty]
    private string _label = string.Empty;

    [ObservableProperty]
    private double _x;

    [ObservableProperty]
    private double _y;

    [ObservableProperty]
    private double _width = 40;

    [ObservableProperty]
    private double _height = 40;

    [ObservableProperty]
    private int _heatLevel;

    [ObservableProperty]
    private int _pressCount;

    [ObservableProperty]
    private Brush _background = Brushes.White;

    [ObservableProperty]
    private Brush _foreground = Brushes.Black;

    partial void OnHeatLevelChanged(int value)
    {
        // Update background color based on heat level
        Background = GetHeatColor(value);
        
        // Update foreground color for contrast
        Foreground = value > 5 ? Brushes.White : Brushes.Black;
    }

    private Brush GetHeatColor(int level)
    {
        return level switch
        {
            0 => Brushes.White,
            1 => new SolidColorBrush(Color.FromRgb(227, 242, 253)), // Cool blue
            2 => new SolidColorBrush(Color.FromRgb(187, 222, 251)),
            3 => new SolidColorBrush(Color.FromRgb(144, 202, 249)),
            4 => new SolidColorBrush(Color.FromRgb(100, 181, 246)),
            5 => new SolidColorBrush(Color.FromRgb(66, 165, 245)),
            6 => new SolidColorBrush(Color.FromRgb(156, 39, 176)), // Purple
            7 => new SolidColorBrush(Color.FromRgb(233, 30, 99)), // Pink
            8 => new SolidColorBrush(Color.FromRgb(244, 67, 54)), // Red
            9 => new SolidColorBrush(Color.FromRgb(255, 87, 34)), // Orange
            10 => new SolidColorBrush(Color.FromRgb(255, 235, 59)), // Yellow
            _ => Brushes.White
        };
    }
}
