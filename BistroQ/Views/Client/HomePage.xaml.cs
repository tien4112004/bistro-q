using BistroQ.Core.Models.Entities;
using BistroQ.ViewModels.Client;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BistroQ.Views.Client
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public HomePageViewModel ViewModel { get; }
        public List<OrderDetail> OrderDetails = new List<OrderDetail>
        {
            new OrderDetail
            {
                OrderDetailId = 1,
                OrderId = null,
                ProductId = 1,
                Quantity = 1,
                PriceAtPurchase = 1,
                Product = new Product
                {
                    ProductId = 1,
                    Name = "A long",
                }
            },
            new OrderDetail
            {
                OrderDetailId = 1,
                OrderId = null,
                ProductId = 1,
                Quantity = 1,
                PriceAtPurchase = 1,
                Product = new Product
                {
                    ProductId = 1,
                    Name = "A very long description A very long description A very long description A very long description",
                }
            },
                        new OrderDetail
            {
                OrderDetailId = 1,
                OrderId = null,
                ProductId = 1,
                Quantity = 1,
                PriceAtPurchase = 1,
                Product = new Product
                {
                    ProductId = 1,
                    Name = "A long",
                }
            },
            new OrderDetail
            {
                OrderDetailId = 1,
                OrderId = null,
                ProductId = 1,
                Quantity = 1,
                PriceAtPurchase = 1,
                Product = new Product
                {
                    ProductId = 1,
                    Name = "A very long description A very long description A very long description A very long description",
                }
            },
                        new OrderDetail
            {
                OrderDetailId = 1,
                OrderId = null,
                ProductId = 1,
                Quantity = 1,
                PriceAtPurchase = 1,
                Product = new Product
                {
                    ProductId = 1,
                    Name = "A long",
                }
            },
            new OrderDetail
            {
                OrderDetailId = 1,
                OrderId = null,
                ProductId = 1,
                Quantity = 1,
                PriceAtPurchase = 1,
                Product = new Product
                {
                    ProductId = 1,
                    Name = "A very long description A very long description A very long description A very long description",
                }
            },
                        new OrderDetail
            {
                OrderDetailId = 1,
                OrderId = null,
                ProductId = 1,
                Quantity = 1,
                PriceAtPurchase = 1,
                Product = new Product
                {
                    ProductId = 1,
                    Name = "A long",
                }
            },
            new OrderDetail
            {
                OrderDetailId = 1,
                OrderId = null,
                ProductId = 1,
                Quantity = 1,
                PriceAtPurchase = 1,
                Product = new Product
                {
                    ProductId = 1,
                    Name = "A very long description A very long description A very long description A very long description",
                }
            },
        };

        public HomePage()
        {
            this.ViewModel = App.GetService<HomePageViewModel>();
            this.InitializeComponent();
        }
    }
}
