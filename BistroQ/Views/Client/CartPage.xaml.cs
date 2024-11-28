using BistroQ.Core.Entities;
using BistroQ.ViewModels.Client;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;

namespace BistroQ.Views.Client;

public sealed partial class CartPage : Page, INotifyPropertyChanged
{
    public OrderCartViewModel ViewModel { get; }

    public bool CartIsEmpty => ViewModel?.CartItems == null || ViewModel.CartItems.Count == 0;

    public ICommand RemoveProductFromCartCommand { get; }

    public CartPage(OrderCartViewModel orderCartViewModel)
    {
        ViewModel = orderCartViewModel;
        this.InitializeComponent();

        if (ViewModel != null)
        {
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            if (ViewModel.CartItems != null)
            {
                ViewModel.CartItems.CollectionChanged += CartItems_CollectionChanged;
            }
        }

        RemoveProductFromCartCommand = new RelayCommand<OrderItem>(RemoveProductFromCart);
    }

    private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ViewModel.CartItems))
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CartIsEmpty)));
            if (ViewModel.CartItems != null)
            {
                ViewModel.CartItems.CollectionChanged += CartItems_CollectionChanged;
            }
        }
    }

    private void CartItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CartIsEmpty)));
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void RemoveProductFromCart(OrderItem item)
    {
        if (item == null)
        {
            return;
        }
        ViewModel.CartItems.Remove(item);
    }
}
