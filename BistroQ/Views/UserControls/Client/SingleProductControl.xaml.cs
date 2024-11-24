using BistroQ.Core.Entities;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BistroQ.Views.UserControls.Client;

public sealed partial class SingleProductControl : UserControl
{
    public Product Product { get; set; }

    public SingleProductControl()
    {
        this.InitializeComponent();
    }

    public SingleProductControl(Product product) : this()
    {
        this.Product = product;
    }
}
