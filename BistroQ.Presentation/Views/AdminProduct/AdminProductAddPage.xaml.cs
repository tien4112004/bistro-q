using BistroQ.Presentation.ViewModels.AdminProduct;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

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
}