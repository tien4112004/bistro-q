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
using BistroQ.Core.Dtos.Tables;
using System.Diagnostics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BistroQ.Views.UserControls.Zones;

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
        if (e.ClickedItem is TableDto table)
        {
            TableSelectionChanged?.Invoke(this, table.TableId);
        }
    }
}