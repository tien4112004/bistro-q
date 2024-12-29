using BistroQ.Presentation.ViewModels.Client;
using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Presentation.Views.UserControls.Client;

public sealed partial class ClientNutritionControl : UserControl
{
    public OrderCartViewModel ViewModel;

    public ClientNutritionControl(OrderCartViewModel viewModel)
    {
        ViewModel = viewModel;
        this.InitializeComponent();
    }
}
