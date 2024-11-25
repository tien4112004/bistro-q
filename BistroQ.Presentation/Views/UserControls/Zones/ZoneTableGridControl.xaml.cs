using BistroQ.Domain.Dtos.Tables;
using BistroQ.Presentation.ViewModels.CashierTable;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Presentation.Views.UserControls.Zones;

public sealed partial class ZoneTableGridControl : UserControl
{
    public ZoneTableGridControl()
    {
        this.InitializeComponent();
    }

    public ZoneTableGridViewModel ViewModel { get; set; }

    public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                nameof(ViewModel),
                typeof(ZoneTableGridViewModel),
                typeof(ZoneTableGridControl),
                new PropertyMetadata(null));


    public event EventHandler<int?> TableSelectionChanged;

    private void OnTableClicked(object sender, ItemClickEventArgs e)
    {
        if (e.ClickedItem is TableResponse table)
        {
            TableSelectionChanged?.Invoke(this, table.TableId);
        }
    }
}
