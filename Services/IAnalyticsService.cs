using System;
using System.Threading.Tasks;

namespace KeyboardHeatmapPro.Services;

public interface IAnalyticsService
{
    Task<KeyboardStatistics> GetStatisticsAsync(string timeRange);
}

public class KeyboardStatistics
{
    public long TotalKeystrokes { get; set; }
    public double AverageDailyUsage { get; set; }
    public string MostActiveKey { get; set; } = string.Empty;
    public TimeSpan PeakUsageTime { get; set; }
}
