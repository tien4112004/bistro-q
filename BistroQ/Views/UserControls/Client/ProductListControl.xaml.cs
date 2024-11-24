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
            null);

        public ProductListViewModel ViewModel { get; set; }

        public ProductListControl()
        {
            this.InitializeComponent();
        }

        private void CategoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel.ChangeCategoryCommand.CanExecute(null))
            {
                ViewModel.ChangeCategoryCommand.Execute(null);
            }
        }
    }
}
