using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BistroQ.ViewModels.Client;

public partial class HomePageViewModel : ObservableRecipient
{
    [ObservableProperty]
    private bool _isOrdering;
    public ICommand StartOrderCommand { get; }
    public HomePageViewModel()
    {
        StartOrderCommand = new AsyncRelayCommand(StartOrder);
    }

    private async Task StartOrder()
    {
        IsOrdering = true;
    }
}
