using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Presentation.Views.UserControls.Skeleton;

public sealed partial class RectangleSkeleton : UserControl
{
    public static readonly DependencyProperty IsLoadingProperty =
        DependencyProperty.Register(nameof(IsLoading), typeof(bool), typeof(RectangleSkeleton),
            new PropertyMetadata(true, OnIsLoadingChanged));

    public static readonly DependencyProperty WidthProperty =
        DependencyProperty.Register(nameof(Width), typeof(double), typeof(RectangleSkeleton),
            new PropertyMetadata(200.0));

    public static readonly DependencyProperty HeightProperty =
        DependencyProperty.Register(nameof(Height), typeof(double), typeof(RectangleSkeleton),
            new PropertyMetadata(20.0));

    public bool IsLoading
    {
        get => (bool)GetValue(IsLoadingProperty);
        set => SetValue(IsLoadingProperty, value);
    }

    public double Width
    {
        get => (double)GetValue(WidthProperty);
        set => SetValue(WidthProperty, value);
    }

    public double Height
    {
        get => (double)GetValue(HeightProperty);
        set => SetValue(HeightProperty, value);
    }

    public RectangleSkeleton()
    {
        this.InitializeComponent();
        this.Loaded += Skeleton_Loaded;
        this.Unloaded += Skeleton_Unloaded;
    }

    private void Skeleton_Loaded(object sender, RoutedEventArgs e)
    {
        if (IsLoading)
        {
            StartAnimation();
        }
    }

    private void Skeleton_Unloaded(object sender, RoutedEventArgs e)
    {
        StopAnimation();
    }

    private static void OnIsLoadingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var skeleton = (RectangleSkeleton)d;
        if ((bool)e.NewValue)
        {
            skeleton.StartAnimation();
        }
        else
        {
            skeleton.StopAnimation();
        }
    }

    private void StartAnimation()
    {
        PulseAnimation?.Begin();
    }

    private void StopAnimation()
    {
        PulseAnimation?.Stop();
    }
}
