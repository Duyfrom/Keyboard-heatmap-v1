using System;

namespace KeyboardHeatmapPro.Models;

public class Session
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public TimeSpan Duration => EndTime - StartTime;
    public int TotalKeystrokes { get; set; }
    public string ApplicationContext { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}
