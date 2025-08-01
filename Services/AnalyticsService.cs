using System;
using System.Linq;
using System.Threading.Tasks;

namespace KeyboardHeatmapPro.Services;

public class AnalyticsService : IAnalyticsService
{
    private readonly IDataService _dataService;
    
    public AnalyticsService(IDataService dataService)
    {
        _dataService = dataService;
    }
    
    public async Task<KeyboardStatistics> GetStatisticsAsync(string timeRange)
    {
        var (from, to) = GetDateRange(timeRange);
        var keystrokes = await _dataService.GetKeystrokesAsync(from, to);
        
        var keystrokeList = keystrokes.ToList();
        var totalDays = (to - from).TotalDays;
        
        var stats = new KeyboardStatistics
        {
            TotalKeystrokes = keystrokeList.Count,
            AverageDailyUsage = totalDays > 0 ? keystrokeList.Count / totalDays : 0
        };
        
        // Find most active key
        if (keystrokeList.Any())
        {
            var mostActive = keystrokeList
                .GroupBy(k => k.Key)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault();
            
            stats.MostActiveKey = mostActive?.Key ?? "None";
            
            // Find peak usage time
            var peakHour = keystrokeList
                .GroupBy(k => k.Timestamp.Hour)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault();
            
            if (peakHour != null)
            {
                stats.PeakUsageTime = TimeSpan.FromHours(peakHour.Key);
            }
        }
        
        return stats;
    }
    
    private (DateTime from, DateTime to) GetDateRange(string timeRange)
    {
        var now = DateTime.Now;
        var to = now;
        var from = timeRange switch
        {
            "7D" => now.AddDays(-7),
            "30D" => now.AddDays(-30),
            "90D" => now.AddDays(-90),
            "1Y" => now.AddYears(-1),
            _ => DateTime.MinValue
        };
        
        return (from, to);
    }
}
