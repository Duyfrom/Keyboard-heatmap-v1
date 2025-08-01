using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KeyboardHeatmapPro.Models;

namespace KeyboardHeatmapPro.Services;

public interface IDataService
{
    Task SaveKeystrokeAsync(KeystrokeData data);
    Task<IEnumerable<KeystrokeData>> GetKeystrokesAsync(DateTime from, DateTime to);
    IEnumerable<Session> GetSessions();
    void DeleteSession(Session session);
    Task<long> GetTotalKeystrokeCountAsync();
    Task CleanupOldDataAsync(int daysToKeep);
}

public class KeystrokeData
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime Timestamp { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Application { get; set; } = string.Empty;
    public Guid SessionId { get; set; }
}
