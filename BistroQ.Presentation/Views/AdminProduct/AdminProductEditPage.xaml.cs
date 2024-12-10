using BistroQ.Presentation.ViewModels.AdminProduct;
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
        ProductEditPage_CategoryComboBox.SelectedValue = ViewModel.Form.CategoryId;
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
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

    private void ProductEditPage_CategoryComboBox_GettingFocus(Microsoft.UI.Xaml.UIElement sender, Microsoft.UI.Xaml.Input.GettingFocusEventArgs args)
    {
        ViewModel.Form.ResetError(nameof(ViewModel.Form.CategoryId));
    }

    private void Name_GettingFocus(Microsoft.UI.Xaml.UIElement sender, Microsoft.UI.Xaml.Input.GettingFocusEventArgs args)
    {
        ViewModel.Form.ResetError(nameof(ViewModel.Form.Name));
    }

    private void Unit_GettingFocus(Microsoft.UI.Xaml.UIElement sender, Microsoft.UI.Xaml.Input.GettingFocusEventArgs args)
    {
        ViewModel.Form.ResetError(nameof(ViewModel.Form.Unit));
    }

    private void ProductEditPage_PriceNumberBox_GettingFocus(Microsoft.UI.Xaml.UIElement sender, Microsoft.UI.Xaml.Input.GettingFocusEventArgs args)
    {
        ViewModel.Form.ResetError(nameof(ViewModel.Form.Price));
    }

    private void ProductEditPage_DiscountPriceNumberBox_GettingFocus(Microsoft.UI.Xaml.UIElement sender, Microsoft.UI.Xaml.Input.GettingFocusEventArgs args)
    {
        ViewModel.Form.ResetError(nameof(ViewModel.Form.DiscountPrice));
    }
}