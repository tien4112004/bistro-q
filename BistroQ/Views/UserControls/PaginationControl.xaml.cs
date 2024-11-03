using BistroQ.Core.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Views.UserControls;
public partial class PaginationControl : UserControl
{
    public static readonly DependencyProperty PaginationProperty = DependencyProperty.Register(
            "Pagination",
            typeof(Pagination),
            typeof(PaginationControl),
            new PropertyMetadata(new Pagination
            {
                TotalItems = 0,
                CurrentPage = 1,
                TotalPages = 0
            }, OnPaginationChanged));

    public Pagination Pagination
    {
        get => (Pagination)GetValue(PaginationProperty);
        set => SetValue(PaginationProperty, value);
    }

    private static void OnPaginationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is PaginationControl control && e.NewValue is Pagination pagination)
        {
            control.UpdatePagination(pagination);
        }
    }

    public PaginationControl()
    {
        this.InitializeComponent();
    }

    private void UpdatePagination(Pagination pagination)
    {
        var item = RowsPerPageSelection.Items
            .Cast<ComboBoxItem>()
            .FirstOrDefault(x => x.Content.ToString() == pagination.PageSize.ToString());

        if (item != null)
        {
            RowsPerPageSelection.SelectedItem = item;
        }
    }

    private void PreviousButton_Click(object sender, RoutedEventArgs e)
    {
        if (Pagination.CurrentPage > 1)
        {
            Pagination.CurrentPage--;
        }
    }

    private void NextButton_Click(object sender, RoutedEventArgs e)
    {
        if (Pagination.CurrentPage < Pagination.TotalPages)
        {
            Pagination.CurrentPage++;
        }
    }

    private void FirstPageButton_Click(object sender, RoutedEventArgs e)
    {
        Pagination.CurrentPage = 1;
    }

    private void LastPageButton_Click(object sender, RoutedEventArgs e)
    {
        Pagination.CurrentPage = Pagination.TotalPages;
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
}
