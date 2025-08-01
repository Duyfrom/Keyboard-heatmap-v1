using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyboardHeatmapPro.Services;

public class ReportService : IReportService
{
    private readonly IDataService _dataService;
    private readonly IAnalyticsService _analyticsService;
    
    public ReportService(IDataService dataService, IAnalyticsService analyticsService)
    {
        _dataService = dataService;
        _analyticsService = analyticsService;
    }
    
    public async Task<string> GenerateReportAsync(string reportType, DateTime startDate, DateTime endDate)
    {
        var sb = new StringBuilder();
        
        // Header
        sb.AppendLine($"# Keyboard Heatmap Report");
        sb.AppendLine($"**Report Type:** {reportType}");
        sb.AppendLine($"**Period:** {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}");
        sb.AppendLine($"**Generated:** {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        sb.AppendLine();
        
        // Get data
        var keystrokes = await _dataService.GetKeystrokesAsync(startDate, endDate);
        var totalKeystrokes = keystrokes.Count();
        
        // Summary
        sb.AppendLine("## Summary");
        sb.AppendLine($"- Total Keystrokes: {totalKeystrokes:N0}");
        sb.AppendLine($"- Daily Average: {totalKeystrokes / Math.Max(1, (endDate - startDate).TotalDays):N0}");
        sb.AppendLine();
        
        // Top Keys
        sb.AppendLine("## Top 10 Most Pressed Keys");
        var topKeys = keystrokes
            .GroupBy(k => k.Key)
            .OrderByDescending(g => g.Count())
            .Take(10)
            .Select((g, i) => $"{i + 1}. {g.Key}: {g.Count():N0} presses ({g.Count() * 100.0 / totalKeystrokes:F1}%)");
        
        foreach (var key in topKeys)
        {
            sb.AppendLine(key);
        }
        
        return sb.ToString();
    }
    
    public async Task ExportReportAsync(string report, string format, string filePath)
    {
        switch (format.ToLower())
        {
            case "txt":
            case "md":
                await File.WriteAllTextAsync(filePath, report);
                break;
                
            case "html":
                var html = ConvertToHtml(report);
                await File.WriteAllTextAsync(filePath, html);
                break;
                
            default:
                throw new NotSupportedException($"Format {format} is not supported");
        }
    }
    
    private string ConvertToHtml(string markdown)
    {
        // Simple markdown to HTML conversion
        var html = markdown
            .Replace("# ", "<h1>")
            .Replace("\n## ", "</h1>\n<h2>")
            .Replace("\n- ", "\n<li>")
            .Replace("**", "<strong>")
            .Replace("\n", "<br>\n");
        
        return $@"<!DOCTYPE html>
<html>
<head>
    <title>Keyboard Heatmap Report</title>
    <style>
        body {{ font-family: Arial, sans-serif; margin: 40px; }}
        h1, h2 {{ color: #2196F3; }}
        li {{ margin: 5px 0; }}
    </style>
</head>
<body>
    {html}
</body>
</html>";
    }
}
