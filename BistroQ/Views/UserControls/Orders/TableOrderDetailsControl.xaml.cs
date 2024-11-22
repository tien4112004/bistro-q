using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using BistroQ.ViewModels.CashierTable;
using BistroQ.Models;
using System.Diagnostics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

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
