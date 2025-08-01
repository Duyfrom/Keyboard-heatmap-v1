using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KeyboardHeatmapPro.Services;

namespace KeyboardHeatmapPro.ViewModels;

public partial class AnalyticsViewModel : ViewModelBase
{
    private readonly IAnalyticsService _analyticsService;
    private readonly IDataService _dataService;

    [ObservableProperty]
    private long _totalKeystrokes;

    [ObservableProperty]
    private double _averageDailyUsage;

    [ObservableProperty]
    private string _mostActiveKey = string.Empty;

    [ObservableProperty]
    private TimeSpan _peakUsageTime;

    [ObservableProperty]
    private string _selectedTimeRange = "7D";

    public ObservableCollection<string> TimeRanges { get; } = new()
    {
        "7D", "30D", "90D", "1Y", "All Time"
    };

    public AnalyticsViewModel(IAnalyticsService analyticsService, IDataService dataService)
    {
        _analyticsService = analyticsService;
        _dataService = dataService;
        Title = "Analytics";
    }

    [RelayCommand]
    private async Task LoadAnalytics()
    {
        IsBusy = true;
        try
        {
            var stats = await _analyticsService.GetStatisticsAsync(SelectedTimeRange);
            TotalKeystrokes = stats.TotalKeystrokes;
            AverageDailyUsage = stats.AverageDailyUsage;
            MostActiveKey = stats.MostActiveKey;
            PeakUsageTime = stats.PeakUsageTime;
        }
        finally
        {
            IsBusy = false;
        }
    }

    partial void OnSelectedTimeRangeChanged(string value)
    {
        _ = LoadAnalytics();
    }
}
