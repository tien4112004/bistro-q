using System.Diagnostics;

namespace BistroQ.Presentation.Helpers;

public static class DebugLogger
{
    private static readonly Dictionary<string, Stopwatch> _operationTimers = new();

    public static void LogOperation(string component, string operation, string details = "")
    {
        var timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
        var threadId = Environment.CurrentManagedThreadId;
        Debug.WriteLine($"[{timestamp}] [{threadId}] {component} - {operation} {details}");
    }

    public static void StartOperation(string operationId)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        _operationTimers[operationId] = stopwatch;
        LogOperation("Timer", $"Started operation {operationId}");
    }

    public static void EndOperation(string operationId)
    {
        if (_operationTimers.TryGetValue(operationId, out var stopwatch))
        {
            stopwatch.Stop();
            LogOperation("Timer", $"Ended operation {operationId}", $"Duration: {stopwatch.ElapsedMilliseconds}ms");
            _operationTimers.Remove(operationId);
        }
    }
}
