using BistroQ.Core.Entities;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BistroQ.Views.UserControls.Client;

public sealed partial class CartItemControl : UserControl
{
    public OrderDetail OrderItem { get; set; }

    public CartItemControl()
    {
        OrderItem = new OrderDetail();
        this.InitializeComponent();
    }

    public CartItemControl(OrderDetail orderItem) : this()
    {
        this.OrderItem = orderItem;
        this.DataContext = this;
    }
}
