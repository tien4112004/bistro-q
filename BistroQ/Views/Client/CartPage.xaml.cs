using BistroQ.ViewModels.Client;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;

namespace BistroQ.Views.Client;

public sealed partial class CartPage : Page, INotifyPropertyChanged
{
    public OrderCartViewModel ViewModel { get; }

    public bool CartIsEmpty => ViewModel?.CartItems == null || ViewModel.CartItems.Count == 0;

    public CartPage(OrderCartViewModel orderCartViewModel)
    {
        ViewModel = orderCartViewModel;
        DataContext = ViewModel;
        Debug.WriteLine(2);
        this.InitializeComponent();

        if (ViewModel != null)
        {
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            if (ViewModel.CartItems != null)
            {
                ViewModel.CartItems.CollectionChanged += CartItems_CollectionChanged;
            }
        }
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
}
