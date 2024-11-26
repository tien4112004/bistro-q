using BistroQ.Presentation.ViewModels.Commons;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;
using System.Data;

namespace BistroQ.Presentation.Views.UserControls;

public partial class PaginationControl : UserControl
{
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

        Pagination.PropertyChanged += OnPaginationPropertyChanged;
    }

    private static void OnPaginationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is PaginationControl control)
        {
            if (e.OldValue is PaginationViewModel oldPagination)
            {
                oldPagination.PropertyChanged -= control.OnPaginationPropertyChanged;
            }

            if (e.NewValue is PaginationViewModel newPagination)
            {
                newPagination.PropertyChanged += control.OnPaginationPropertyChanged;
                control.UpdatePagination(newPagination);
            }

            control.UpdateCommandStates();
        }
    }

    private void OnPaginationPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        UpdateCommandStates();
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
                Pagination.PageSize = pageSize;
            }
        }
    }

    private void FirstPage() => Pagination.CurrentPage = 1;
    private bool CanFirstPage() => Pagination?.CurrentPage > 1;

    private void PreviousPage() => Pagination.CurrentPage--;
    private bool CanPreviousPage() => Pagination?.CurrentPage > 1;

    private void NextPage() => Pagination.CurrentPage++;
    private bool CanNextPage() => Pagination?.CurrentPage < Pagination?.TotalPages;

    private void LastPage() => Pagination.CurrentPage = Pagination.TotalPages;
    private bool CanLastPage() => Pagination?.CurrentPage < Pagination?.TotalPages;
}
