﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using BistroQ.ViewModels.CashierTable;



namespace BistroQ.Views.UserControls.Orders;

public sealed partial class TableOrderDetailsControl : UserControl
{
    public TableOrderDetailViewModel ViewModel { get; set;  }

    public static readonly DependencyProperty ViewModelProperty =
        DependencyProperty.Register(
            nameof(ViewModel),
            typeof(TableOrderDetailViewModel),
            typeof(TableBillSummaryControl),
            new PropertyMetadata(null));

    public TableOrderDetailsControl()
    {
        this.InitializeComponent();
    }

    public event EventHandler<int?> CheckoutRequested;

    private void TableBillSummaryControl_CheckoutRequested(object sender, EventArgs e)
    {
        CheckoutRequested?.Invoke(this, ViewModel.Order.TableId);
    }
}
