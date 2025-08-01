using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KeyboardHeatmapPro.Models;
using KeyboardHeatmapPro.Services;

namespace KeyboardHeatmapPro.ViewModels;

public partial class TimelineViewModel : ViewModelBase
{
    private readonly IDataService _dataService;

    [ObservableProperty]
    private ObservableCollection<Session> _sessions = new();

    [ObservableProperty]
    private Session? _selectedSession;

    public TimelineViewModel(IDataService dataService)
    {
        _dataService = dataService;
        Title = "Timeline";
        LoadSessions();
    }

    private void LoadSessions()
    {
        Sessions = new ObservableCollection<Session>(_dataService.GetSessions());
    }

    [RelayCommand]
    private void DeleteSession(Session session)
    {
        Sessions.Remove(session);
        _dataService.DeleteSession(session);
    }
}
