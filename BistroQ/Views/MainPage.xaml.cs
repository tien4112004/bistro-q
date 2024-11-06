using BistroQ.Core.Contracts.Services;
using BistroQ.Core.Models.Entities;
using BistroQ.ViewModels;

using Microsoft.UI.Xaml.Controls;
using System.Diagnostics;

namespace BistroQ.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel { get; }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
    }

}
