using BistroQ.Presentation.Messages;
using BistroQ.Presentation.ViewModels.Commons;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Data;

namespace BistroQ.Presentation.Views.UserControls;

public partial class PaginationControl : UserControl
{
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

        _messenger.Register<PaginationChangedMessage>(this, (r, m) =>
        {
            Pagination.TotalItems = m.TotalItems;
            Pagination.CurrentPage = m.CurrentPage;
            Pagination.TotalPages = m.TotalPages;
            UpdateCommandStates();
        });
    }

    private static void OnPaginationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is PaginationControl control)
        {
            control.UpdateCommandStates();
            control.UpdatePagination((PaginationViewModel)e.NewValue);
        }
    }

    private void UpdateCommandStates()
    {
        FirstPageCommand.NotifyCanExecuteChanged();
        PreviousPageCommand.NotifyCanExecuteChanged();
        NextPageCommand.NotifyCanExecuteChanged();
        LastPageCommand.NotifyCanExecuteChanged();
    }

    private void UpdatePagination(PaginationViewModel pagination)
    {
        var item = RowsPerPageSelection.Items
            .Cast<ComboBoxItem>()
            .FirstOrDefault(x => x.Content.ToString() == pagination.PageSize.ToString());
        if (item != null)
        {
            RowsPerPageSelection.SelectedItem = item;
        }
    }

    private void RowsPerPageSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is ComboBox comboBox && comboBox.SelectedItem is ComboBoxItem item)
        {
            if (int.TryParse(item.Content.ToString(), out int pageSize))
            {
                _messenger.Send(new PageSizeChangedMessage(pageSize));
            }
        }
    }

    private void FirstPage() => _messenger.Send(new CurrentPageChangedMessage(1));
    private bool CanFirstPage() => Pagination?.CurrentPage > 1;

    private void PreviousPage() => _messenger.Send(new CurrentPageChangedMessage(Pagination.CurrentPage - 1));
    private bool CanPreviousPage() => Pagination?.CurrentPage > 1;

    private void NextPage() => _messenger.Send(new CurrentPageChangedMessage(Pagination.CurrentPage + 1));
    private bool CanNextPage() => Pagination?.CurrentPage < Pagination?.TotalPages;

    private void LastPage() => _messenger.Send(new CurrentPageChangedMessage(Pagination.TotalPages));
    private bool CanLastPage() => Pagination?.CurrentPage < Pagination?.TotalPages;
}