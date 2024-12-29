using BistroQ.Presentation.Helpers;
using BistroQ.Presentation.Models;
using BistroQ.Presentation.ViewModels.AdminProduct;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Windows.Storage.Pickers;
using WinRT.Interop;

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
    }

    private async void AdminProductEditPage_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        await ViewModel.LoadCategoriesAsync();
        ProductEditPage_CategoryComboBox.SelectedValue = ViewModel.Form.CategoryId;
    }

    private void OnNavigateBack(object sender, EventArgs e)
    {
        ViewModel.NavigateBack();
    }

    private void AdminProductEditPage_CancelButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        ViewModel.NavigateBack();
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

    private void ProductEditPage_CaloriesNumberBox_GettingFocus(Microsoft.UI.Xaml.UIElement sender, Microsoft.UI.Xaml.Input.GettingFocusEventArgs args)
    {
        ViewModel.Form.ResetError(nameof(ViewModel.Form.Calories));
    }

    private void ProductEditPage_FatNumberBox_GettingFocus(Microsoft.UI.Xaml.UIElement sender, Microsoft.UI.Xaml.Input.GettingFocusEventArgs args)
    {
        ViewModel.Form.ResetError(nameof(ViewModel.Form.Fat));
    }

    private void ProductEditPage_FiberNumberBox_GettingFocus(Microsoft.UI.Xaml.UIElement sender, Microsoft.UI.Xaml.Input.GettingFocusEventArgs args)
    {
        ViewModel.Form.ResetError(nameof(ViewModel.Form.Fiber));
    }

    private void ProductEditPage_ProteinNumberBox_GettingFocus(Microsoft.UI.Xaml.UIElement sender, Microsoft.UI.Xaml.Input.GettingFocusEventArgs args)
    {
        ViewModel.Form.ResetError(nameof(ViewModel.Form.Protein));
    }

    private void ProductEditPage_CarbohydratesNumberBox_GettingFocus(Microsoft.UI.Xaml.UIElement sender, Microsoft.UI.Xaml.Input.GettingFocusEventArgs args)
    {
        ViewModel.Form.ResetError(nameof(ViewModel.Form.Protein));
    }

    private async void ProductEditPage_SelectImageButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        FileOpenPicker fileOpenPicker = new()
        {
            ViewMode = PickerViewMode.Thumbnail,
            FileTypeFilter = { ".jpg", ".jpeg", ".png", ".gif" },
        };

        var windowHandle = WindowNative.GetWindowHandle(App.MainWindow);
        InitializeWithWindow.Initialize(fileOpenPicker, windowHandle);

        var file = await fileOpenPicker.PickSingleFileAsync();

        if (file != null)
        {
            ViewModel.Form.ImageUrl = file.Path;
            ViewModel.IsProcessing = true;
            ViewModel.Form.ImageFile = await FileWrapper.FromStorageFileAsync(file);
            ViewModel.IsProcessing = false;
        }
    }

    private void Button_PointerEntered(object sender, PointerRoutedEventArgs e)
    {
        (sender as UIElement)?.ChangeCursor(CursorType.Hand);
    }

    private void Button_PointerExited(object sender, PointerRoutedEventArgs e)
    {
        (sender as UIElement)?.ChangeCursor(CursorType.Arrow);
    }
}