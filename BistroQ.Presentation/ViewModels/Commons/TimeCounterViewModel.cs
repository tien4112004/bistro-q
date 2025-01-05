using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;

namespace BistroQ.Presentation.ViewModels.Commons;

/// <summary>
/// ViewModel responsible for managing a time counter with elapsed time display.
/// Provides functionality for starting, stopping, and resetting a timer,
/// as well as formatting the elapsed time for display.
/// </summary>
/// <remarks>
/// Implements ObservableObject to provide change notifications for timer properties.
/// Uses DispatcherTimer for UI thread-safe time updates.
/// </remarks>
public partial class TimeCounterViewModel : ObservableObject
{
    #region Private Fields
    /// <summary>
    /// Timer instance for tracking elapsed time
    /// </summary>
    private DispatcherTimer _timer;

    /// <summary>
    /// The current elapsed time
    /// </summary>
    private TimeSpan _elapsedTime;

    /// <summary>
    /// The time when the counter was started
    /// </summary>
    private DateTime _startTime;
    #endregion

    #region Public Properties
    /// <summary>
    /// Gets the elapsed time since the counter started
    /// </summary>
    /// <remarks>
    /// Updates the FormattedTime property when changed
    /// </remarks>
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

    /// <summary>
    /// Gets the elapsed time formatted as "hh:mm:ss"
    /// </summary>
    public string FormattedTime => ElapsedTime.ToString(@"hh\:mm\:ss");
    #endregion

    #region Constructor
    public TimeCounterViewModel()
    {
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += Timer_Tick;
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Sets the start time for the counter
    /// </summary>
    /// <param name="startTime">The datetime to set as start time</param>
    /// <exception cref="ArgumentException">Thrown when start time is in the future</exception>
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

    /// <summary>
    /// Sets the start time relative to the current time
    /// </summary>
    /// <param name="timeAgo">The timespan representing how long ago to start the timer</param>
    public void SetStartTime(TimeSpan timeAgo)
    {
        SetStartTime(DateTime.Now - timeAgo);
    }

    /// <summary>
    /// Starts the timer from the current time
    /// </summary>
    public void Start()
    {
        _startTime = DateTime.Now;
        ElapsedTime = TimeSpan.Zero;
        _timer.Start();
    }

    /// <summary>
    /// Stops the timer
    /// </summary>
    public void Stop()
    {
        _timer.Stop();
    }

    /// <summary>
    /// Resets the timer to zero and stops it
    /// </summary>
    public void Reset()
    {
        Stop();
        ElapsedTime = TimeSpan.Zero;
    }

    /// <summary>
    /// Disposes of the timer resources
    /// </summary>
    public void Dispose()
    {
        _timer?.Stop();
        _timer = null;
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Handler for timer tick events
    /// Updates the elapsed time based on the current time
    /// </summary>
    /// <param name="sender">The source of the timer event</param>
    /// <param name="e">Event arguments</param>
    private void Timer_Tick(object sender, object e)
    {
        ElapsedTime = DateTime.Now - _startTime;
    }
    #endregion
}