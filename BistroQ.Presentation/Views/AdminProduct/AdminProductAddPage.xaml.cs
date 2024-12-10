using BistroQ.Presentation.Models;
using BistroQ.Presentation.ViewModels.AdminProduct;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace BistroQ.Presentation.Views.AdminProduct;

public sealed partial class AdminProductAddPage : Page
{
    public AdminProductAddPageViewModel ViewModel { get; set; }

    public AdminProductAddPage()
    {
        ViewModel = App.GetService<AdminProductAddPageViewModel>();
        this.DataContext = ViewModel;
        this.InitializeComponent();

        this.Loaded += AdminProductAddPage_Loaded;
        ViewModel.NavigateBack += OnNavigateBack;

        Unloaded += (s, e) =>
        {
            ViewModel.NavigateBack -= OnNavigateBack;
        };
    }

    private async void AdminProductAddPage_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        await ViewModel.LoadCategoriesAsync();
    }

    private void AdminProductAddPage_CancelButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        Frame.GoBack();
    }

    private void OnNavigateBack(object sender, EventArgs e)
    {
        Frame.GoBack();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
    }

    private async void ProductAddPage_SelectImageButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
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