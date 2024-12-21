using BistroQ.Presentation.ViewModels.Models;
using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Presentation.Views.UserControls.Client;

public sealed partial class ProductDetailControl : UserControl
{
    public ProductDetailControl(ProductViewModel product)
    {
        this.InitializeComponent();
        this.DataContext = product;
    }
}
