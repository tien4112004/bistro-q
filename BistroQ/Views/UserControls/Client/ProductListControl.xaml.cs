using BistroQ.Core.Entities;
using BistroQ.ViewModels.Client;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BistroQ.Views.UserControls.Client
{
    public sealed partial class ProductListControl : UserControl
    {
        public static readonly DependencyProperty ProductListViewModelProperty = DependencyProperty.Register(
            nameof(ProductListViewModel),
            typeof(ProductListViewModel),
            typeof(ProductListControl),
            new PropertyMetadata(null));

        public ProductListViewModel ViewModel
        {
            get => (ProductListViewModel)GetValue(ProductListViewModelProperty);
            set => SetValue(ProductListViewModelProperty, value);
        }

        public ProductListControl()
        {
            this.InitializeComponent();
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

        private void AddToCartButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel != null && sender is Button button && button.DataContext is Product product)
            {
                ViewModel.AddProductToCartCommand.Execute(product);
            }
        }
    }
}
