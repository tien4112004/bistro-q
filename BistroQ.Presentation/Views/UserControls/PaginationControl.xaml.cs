using BistroQ.Presentation.Helpers;
using BistroQ.Presentation.Messages;
using BistroQ.Presentation.ViewModels.Commons;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace BistroQ.Presentation.Views.UserControls;

public partial class PaginationControl : UserControl, IDisposable, IRecipient<PaginationChangedMessage>
{
    private bool _isDisposed = false;
    private readonly IMessenger _messenger = App.GetService<IMessenger>();

    public static readonly DependencyProperty PaginationProperty = DependencyProperty.Register(
        "Pagination",
        typeof(PaginationViewModel),
        typeof(PaginationControl),
        new PropertyMetadata(new PaginationViewModel
        {
            TotalItems = 0,
            CurrentPage = 1,
            TotalPages = 0
        }, OnPaginationChanged));

    public PaginationViewModel Pagination
    {
        get => (PaginationViewModel)GetValue(PaginationProperty);
        set => SetValue(PaginationProperty, value);
    }

    public IRelayCommand FirstPageCommand { get; }
    public IRelayCommand PreviousPageCommand { get; }
    public IRelayCommand NextPageCommand { get; }
    public IRelayCommand LastPageCommand { get; }

    public PaginationControl()
    {
        this.DataContext = this;
        InitializeComponent();

        FirstPageCommand = new RelayCommand(FirstPage, CanFirstPage);
        PreviousPageCommand = new RelayCommand(PreviousPage, CanPreviousPage);
        NextPageCommand = new RelayCommand(NextPage, CanNextPage);
        LastPageCommand = new RelayCommand(LastPage, CanLastPage);

        _messenger.RegisterAll(this);

        this.Unloaded += PaginationControl_Unloaded;
    }
    private static void OnPaginationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is PaginationControl control)
        {
            control.UpdateCommandStates();
        }
    }

    private void UpdateCommandStates()
    {
        FirstPageCommand.NotifyCanExecuteChanged();
        PreviousPageCommand.NotifyCanExecuteChanged();
        NextPageCommand.NotifyCanExecuteChanged();
        LastPageCommand.NotifyCanExecuteChanged();
    }

    private void RowsPerPageSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_isDisposed) return;

        if (sender is ComboBox comboBox && comboBox.SelectedItem is ComboBoxItem item)
        {
            if (int.TryParse(item.Content.ToString(), out int pageSize) && pageSize != Pagination.PageSize)
            {
                _messenger.Send(new PageSizeChangedMessage(pageSize));
                Pagination.CurrentPage = 1;
            }
        }
    }

    private void FirstPage()
    {
        Pagination.CurrentPage = 1;
        _messenger.Send(new CurrentPageChangedMessage(1));
    }
    private bool CanFirstPage() => Pagination?.CurrentPage > 1;

    private void PreviousPage() => _messenger.Send(new CurrentPageChangedMessage(--Pagination.CurrentPage));
    private bool CanPreviousPage() => Pagination?.CurrentPage > 1;

    private void NextPage() => _messenger.Send(new CurrentPageChangedMessage(++Pagination.CurrentPage));
    private bool CanNextPage() => Pagination?.CurrentPage < Pagination?.TotalPages;

    private void LastPage()
    {
        Pagination.CurrentPage = Pagination.TotalPages;
        _messenger.Send(new CurrentPageChangedMessage(Pagination.TotalPages));
    }
    private bool CanLastPage() => Pagination?.CurrentPage < Pagination?.TotalPages;

    private void PaginationControl_Unloaded(object sender, RoutedEventArgs e)
    {
        Dispose();
    }

    public void Receive(PaginationChangedMessage message)
    {
        if (_isDisposed) return;

        Pagination = new PaginationViewModel
        {
            TotalItems = message.TotalItems,
            CurrentPage = message.CurrentPage,
            TotalPages = message.TotalPages
        };
    }

    public void Dispose()
    {
        if (_isDisposed) return;
        _isDisposed = true;

        this.Unloaded -= PaginationControl_Unloaded;
        _messenger.UnregisterAll(this);
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