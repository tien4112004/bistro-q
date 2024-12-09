using BistroQ.Presentation.ViewModels.AdminProduct;
using BistroQ.Presentation.ViewModels.Models;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace BistroQ.Presentation.Views.AdminProduct;

public sealed partial class AdminProductEditPage : Page
{
    public AdminProductEditPageViewModel ViewModel { get; set; }

    public AdminProductEditPage()
    {
        InitializeComponent();
        ViewModel = App.GetService<AdminProductEditPageViewModel>();
        this.DataContext = ViewModel;

        this.Loaded += AdminProductEditPage_Loaded;
        ViewModel.NavigateBack += OnNavigateBack;

        this.Unloaded += (s, e) =>
        {
            ViewModel.NavigateBack -= OnNavigateBack;
        };
    }

    private async void AdminProductEditPage_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        await ViewModel.LoadCategoriesAsync();
        ProductEditPage_CategoryComboBox.SelectedValue = ViewModel.Request.CategoryId;
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        var productDto = e.Parameter as ProductViewModel;
        if (productDto != null)
        {
            ViewModel.Product = productDto;
        }

        base.OnNavigatedTo(e);
    }

    private void OnNavigateBack(object sender, EventArgs e)
    {
        Frame.GoBack();
    }

    private void AdminProductEditPage_CancelButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        Frame.GoBack();
    }
}