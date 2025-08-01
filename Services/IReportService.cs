using System;
using System.Threading.Tasks;

namespace KeyboardHeatmapPro.Services;

public interface IReportService
{
    Task<string> GenerateReportAsync(string reportType, DateTime startDate, DateTime endDate);
    Task ExportReportAsync(string report, string format, string filePath);
}
