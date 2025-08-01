using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LiteDB;
using KeyboardHeatmapPro.Models;

namespace KeyboardHeatmapPro.Services;

public class DataService : IDataService, IDisposable
{
    private readonly LiteDatabase _database;
    private readonly ILiteCollection<KeystrokeData> _keystrokes;
    private readonly ILiteCollection<Session> _sessions;
    
    public DataService()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var dbPath = Path.Combine(appData, "KeyboardHeatmapPro", "data.db");
        Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);
        
        _database = new LiteDatabase(dbPath);
        _keystrokes = _database.GetCollection<KeystrokeData>("keystrokes");
        _sessions = _database.GetCollection<Session>("sessions");
        
        // Create indexes
        _keystrokes.EnsureIndex(x => x.Timestamp);
        _keystrokes.EnsureIndex(x => x.SessionId);
        _sessions.EnsureIndex(x => x.StartTime);
    }
    
    public async Task SaveKeystrokeAsync(KeystrokeData data)
    {
        await Task.Run(() => _keystrokes.Insert(data));
    }
    
    public async Task<IEnumerable<KeystrokeData>> GetKeystrokesAsync(DateTime from, DateTime to)
    {
        return await Task.Run(() => 
            _keystrokes.Find(x => x.Timestamp >= from && x.Timestamp <= to).ToList());
    }
    
    public IEnumerable<Session> GetSessions()
    {
        return _sessions.FindAll().OrderByDescending(s => s.StartTime);
    }
    
    public void DeleteSession(Session session)
    {
        _sessions.Delete(session.Id);
        _keystrokes.DeleteMany(x => x.SessionId == session.Id);
    }
    
    public async Task<long> GetTotalKeystrokeCountAsync()
    {
        return await Task.Run(() => _keystrokes.LongCount());
    }
    
    public async Task CleanupOldDataAsync(int daysToKeep)
    {
        var cutoffDate = DateTime.Now.AddDays(-daysToKeep);
        await Task.Run(() =>
        {
            _keystrokes.DeleteMany(x => x.Timestamp < cutoffDate);
            _sessions.DeleteMany(x => x.EndTime < cutoffDate);
        });
    }
    
    public void Dispose()
    {
        _database?.Dispose();
    }
}
