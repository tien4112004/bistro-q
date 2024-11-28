using BistroQ.Core.Entities;
using BistroQ.ViewModels.Client;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Diagnostics;


namespace BistroQ.Views.UserControls.Client
{
    public sealed partial class ProductListControl : UserControl
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            nameof(ViewModel),
            typeof(ProductListViewModel),
            typeof(ProductListControl),
            new PropertyMetadata(null));

        public ProductListViewModel ViewModel
        {
            get => (ProductListViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        public event EventHandler<Product> AddProductToCart;
        public event EventHandler<Product> ProductSelected;

        public ProductListControl()
        {
            this.InitializeComponent();
            this.Loaded += ProductListControl_Loaded;
        }

        private void ProductListControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (ViewModel == null)
            {
                ViewModel = App.GetService<ProductListViewModel>();
            }
            DataContext = ViewModel;
        }

        private void CategoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel != null && e.AddedItems.Count > 0)
            {
                ViewModel.SelectedCategory = e.AddedItems[0] as Category;
                if (ViewModel.ChangeCategoryCommand.CanExecute(ViewModel.SelectedCategory))
                {
                    ViewModel.ChangeCategoryCommand.Execute(ViewModel.SelectedCategory);
                }
            }
        }

        private void ProductList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is GridViewItem item && item.DataContext is Product product)
            {
                ProductSelected.Invoke(this, product);
                Debug.WriteLine(product);
            }
        }

        private void AddToCartButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Product product)
            {
                AddProductToCart?.Invoke(this, product);
            }
        }
    }
}
