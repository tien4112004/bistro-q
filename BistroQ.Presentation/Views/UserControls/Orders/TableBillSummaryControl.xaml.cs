using BistroQ.Presentation.Contracts.Services;
using BistroQ.Presentation.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace BistroQ.Presentation.Views.UserControls.Orders;

public sealed partial class TableBillSummaryControl : UserControl
{
    private readonly IDialogService _dialogService;

    public TableBillSummaryControl()
    {
        this.InitializeComponent();
        _dialogService = App.GetService<IDialogService>();
    }

    public decimal? Total
    {
        get => (decimal?)GetValue(TotalProperty);
        set => SetValue(TotalProperty, value);
    }

    public string? ButtonText
    {
        get => (string?)GetValue(ButtonTextProperty);
        set => SetValue(ButtonTextProperty, value);
    }

    public bool IsButtonEnabled
    {
        get => (bool)GetValue(IsButtonEnabledProperty);
        set => SetValue(IsButtonEnabledProperty, value);
    }

    public string? DialogTitle
    {
        get => (string?)GetValue(DialogTitleProperty);
        set => SetValue(DialogTitleProperty, value);
    }

    public string? DialogContent
    {
        get => (string?)GetValue(DialogContentProperty);
        set => SetValue(DialogContentProperty, value);
    }

    public bool RequireConfirmation
    {
        get => (bool)GetValue(RequireConfirmationProperty);
        set => SetValue(RequireConfirmationProperty, value);
    }

    public static readonly DependencyProperty DialogTitleProperty =
        DependencyProperty.Register(
            nameof(DialogTitle),
            typeof(string),
            typeof(TableBillSummaryControl),
            new PropertyMetadata(null));

    public static readonly DependencyProperty DialogContentProperty =
        DependencyProperty.Register(
            nameof(DialogContent),
            typeof(string),
            typeof(TableBillSummaryControl),
            new PropertyMetadata(null));

    public static readonly DependencyProperty RequireConfirmationProperty =
        DependencyProperty.Register(
            nameof(RequireConfirmation),
            typeof(bool),
            typeof(TableBillSummaryControl),
            new PropertyMetadata(false));

    public static readonly DependencyProperty TotalProperty =
        DependencyProperty.Register(
            nameof(Total),
            typeof(decimal?),
            typeof(TableBillSummaryControl),
            new PropertyMetadata(null));

    public static readonly DependencyProperty ButtonTextProperty =
        DependencyProperty.Register(
            nameof(ButtonText),
            typeof(string),
            typeof(TableBillSummaryControl),
            new PropertyMetadata(null));

    public static readonly DependencyProperty IsButtonEnabledProperty =
        DependencyProperty.Register(
            nameof(IsButtonEnabled),
            typeof(bool),
            typeof(TableBillSummaryControl),
            new PropertyMetadata(true));

    public event EventHandler CheckoutRequested;

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
        if (!RequireConfirmation)
        {
            CheckoutRequested?.Invoke(this, EventArgs.Empty);
            return;
        }

        var dialog = new ContentDialog
        {
            Title = DialogTitle ?? "Confirm Checkout",
            Content = DialogContent ?? "Are you sure you want to proceed with checkout?",
            PrimaryButtonText = "Yes",
            CloseButtonText = "No",
            DefaultButton = ContentDialogButton.Close
        };

        var result = await _dialogService.ShowDialogAsync(dialog);
        if (result == ContentDialogResult.Primary)
        {
            CheckoutRequested?.Invoke(this, EventArgs.Empty);
        }
    }

    private void Button_PointerEntered(object sender, PointerRoutedEventArgs e)
    {
        (sender as UIElement)?.ChangeCursor(CursorType.Hand);
    }

    private void Button_PointerExited(object sender, PointerRoutedEventArgs e)
    {
        (sender as UIElement)?.ChangeCursor(CursorType.Arrow);
    }
}