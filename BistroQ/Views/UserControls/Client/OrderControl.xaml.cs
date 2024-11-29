using BistroQ.ViewModels.Client;
using Microsoft.UI.Xaml.Controls;
using System.ComponentModel;
using System.Diagnostics;

namespace BistroQ.Views.UserControls.Client;

public sealed partial class OrderControl : UserControl, INotifyPropertyChanged
{
    public OrderCartViewModel ViewModel { get; }

    public bool ProcessingOrdersIsEmpty => ViewModel.ProcessingItems == null || ViewModel.ProcessingItems.Count() == 0;
    public bool CompletedOrdersIsEmpty => ViewModel.CompletedItems == null || ViewModel.CompletedItems.Count() == 0;

    public OrderControl(OrderCartViewModel orderCartViewModel)
    {
        this.InitializeComponent();
        ViewModel = orderCartViewModel;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void TableBillSummaryControl_CheckoutRequested(object sender, EventArgs e)
    {
        Debug.WriteLine("[Debug] Order clicked");
    }
}