namespace BistroQ.Presentation.Helpers;

public static class TaskHelper
{
    public static async Task<T> WithMinimumDelay<T>(Task<T> task, int delayMs = 200)
    {
        await Task.WhenAll(task, Task.Delay(delayMs));
        return await task;
    }

    public static async Task WithMinimumDelay(Task task, int delayMs = 200)
    {
        await Task.WhenAll(task, Task.Delay(delayMs));
        await task;
    }
}
