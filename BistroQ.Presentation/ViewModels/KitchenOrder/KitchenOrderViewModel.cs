using BistroQ.Presentation.Contracts.ViewModels;
using BistroQ.Presentation.ViewModels.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BistroQ.Presentation.ViewModels.KitchenOrder;

public partial class KitchenOrderViewModel : ObservableObject, INavigationAware
{
    public OrderKanbanColumnViewModel PendingColumnVM { get; set; }
    public OrderKanbanColumnViewModel ProgressColumnVM { get; set; }
    
    public KitchenOrderViewModel()
    {
        PendingColumnVM = App.GetService<OrderKanbanColumnViewModel>();
        ProgressColumnVM = App.GetService<OrderKanbanColumnViewModel>();
    }

    public void OnNavigatedFrom()
    {

    }

    public void OnNavigatedTo(object parameter)
    {
        PendingColumnVM.Items.Add(new OrderItemViewModel
        {
            OrderId = "1",
            Status = "Pending",
            ProductId = 1,
            OrderDetailId = 1,
            PriceAtPurchase = 100,
            Quantity = 1,
            Product = new ProductViewModel
            {
                ProductId = 1,
                Name = "Burger",
                Price = 100,
                ImageUrl = "https://placehold.jp/300x200.png"
            }
        });

        PendingColumnVM.Items.Add(new OrderItemViewModel
        {
            OrderId = "1",
            Status = "Pending",
            ProductId = 2,
            OrderDetailId = 2,
            PriceAtPurchase = 200,
            Quantity = 2,
            Product = new ProductViewModel
            {
                ProductId = 2,
                Name = "Pizza",
                Price = 200,
                ImageUrl = "https://placehold.jp/300x200.png"
            }
        });

        ProgressColumnVM.Items.Add(new OrderItemViewModel
        {
            OrderId = "1",
            Status = "Progress",
            ProductId = 3,
            OrderDetailId = 3,
            PriceAtPurchase = 300,
            Quantity = 3,
            Product = new ProductViewModel
            {
                ProductId = 3,
                Name = "Pasta",
                Price = 300,
                ImageUrl = "https://placehold.jp/300x200.png"
            }
        });
    }
}