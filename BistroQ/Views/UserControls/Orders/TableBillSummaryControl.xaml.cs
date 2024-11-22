﻿using System;
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
using BistroQ.Views.UserControls.Zones;
using System.Diagnostics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BistroQ.Views.UserControls.Orders;

public sealed partial class TableBillSummaryControl : UserControl
{
    public TableBillSummaryControl()
    {
        this.InitializeComponent();
    }

    public decimal? Total
    {
        get => (decimal?)GetValue(TotalProperty);
        set => SetValue(TotalProperty, value);
    }

    public static readonly DependencyProperty TotalProperty = 
        DependencyProperty.Register(
            nameof(Total),
            typeof(decimal?),
            typeof(TableBillSummaryControl),
            new PropertyMetadata(null));

    public event EventHandler CheckoutRequested;

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        CheckoutRequested.Invoke(this, EventArgs.Empty);
    }
}
