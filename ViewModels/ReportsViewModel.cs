using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KeyboardHeatmapPro.Services;

namespace KeyboardHeatmapPro.ViewModels;

public partial class ReportsViewModel : ViewModelBase
{
    private readonly IReportService _reportService;

    [ObservableProperty]
    private string _selectedReportType = "Daily";

    [ObservableProperty]
    private DateTime _startDate = DateTime.Today.AddDays(-7);

    [ObservableProperty]
    private DateTime _endDate = DateTime.Today;

    public string[] ReportTypes { get; } = { "Daily", "Weekly", "Monthly", "Custom" };

    public ReportsViewModel(IReportService reportService)
    {
        _reportService = reportService;
        Title = "Reports";
    }

    [RelayCommand]
    private async Task GenerateReport()
    {
        IsBusy = true;
        try
        {
            await _reportService.GenerateReportAsync(SelectedReportType, StartDate, EndDate);
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task ExportReport()
    {
        // TODO: Implement report export
        await Task.CompletedTask;
    }
}
