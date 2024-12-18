﻿using BistroQ.Presentation.Models;
using BistroQ.Presentation.ViewModels.AdminProduct;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
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
}