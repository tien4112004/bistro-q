using BistroQ.Presentation.ViewModels.CashierTable;
using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Presentation.Views.CashierTable;
public sealed partial class CashierTablePage : Page
{
    public CashierTableViewModel ViewModel { get; }
    public CashierTablePage()
    {
        ViewModel = App.GetService<CashierTableViewModel>();
        this.InitializeComponent();
    }
}