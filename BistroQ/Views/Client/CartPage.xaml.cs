using BistroQ.Core.Entities;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace BistroQ.Views.Client;

public sealed partial class CartPage : Page, INotifyPropertyChanged
{
    public ObservableCollection<OrderDetail> CartItems;

    public bool CartIsEmpty => CartItems == null || CartItems.Count == 0;

    public CartPage()
    {
        this.InitializeComponent();
        CartItems = new ObservableCollection<OrderDetail>
            {
                new OrderDetail
                {
                    OrderDetailId = "123123",
                    Quantity = 12,
                    Product = new Product
                    {
                        Name = "Bun bo",
                        Unit = "Bowl",
                    },
                }
            };
    }

    public event PropertyChangedEventHandler PropertyChanged;
}
