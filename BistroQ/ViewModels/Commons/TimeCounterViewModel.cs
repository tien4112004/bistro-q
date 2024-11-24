using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;

namespace BistroQ.ViewModels.Commons;

public partial class TimeCounterViewModel : ObservableObject
{
    private DispatcherTimer _timer;
    private TimeSpan _elapsedTime;
    private DateTime _startTime;

    public TimeSpan ElapsedTime
    {
        get => _elapsedTime;
        private set
        {
            if (SetProperty(ref _elapsedTime, value))
            {
                OnPropertyChanged(nameof(FormattedTime));
            }
        }
    }

    public string FormattedTime => ElapsedTime.ToString(@"hh\:mm\:ss");

    public TimeCounterViewModel()
    {
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += Timer_Tick;
    }

    public void SetStartTime(DateTime startTime)
    {
        if (startTime > DateTime.Now)
        {
            throw new ArgumentException("Start time cannot be in the future");
        }

        _startTime = startTime;
        ElapsedTime = DateTime.Now - _startTime;

        if (!_timer.IsEnabled)
        {
            _timer.Start();
        }
    }

    public void SetStartTime(TimeSpan timeAgo)
    {
        SetStartTime(DateTime.Now - timeAgo);
    }

    public void Start()
    {
        _startTime = DateTime.Now;
        ElapsedTime = TimeSpan.Zero;
        _timer.Start();
    }

    public void Stop()
    {
        _timer.Stop();
    }

    public void Reset()
    {
        Stop();
        ElapsedTime = TimeSpan.Zero;
    }

    private void Timer_Tick(object sender, object e)
    {
        ElapsedTime = DateTime.Now - _startTime;
    }

    public void Dispose()
    {
        _timer?.Stop();
        _timer = null;
    }
}
