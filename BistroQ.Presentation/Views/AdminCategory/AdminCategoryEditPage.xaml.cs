using BistroQ.Presentation.ViewModels.AdminCategory;
using BistroQ.Presentation.ViewModels.Models;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace BistroQ.Presentation.Views.AdminCategory;

public sealed partial class AdminCategoryEditPage : Page
{
    public AdminCategoryEditPageViewModel ViewModel { get; set; }

    public AdminCategoryEditPage()
    {
        ViewModel = App.GetService<AdminCategoryEditPageViewModel>();
        this.DataContext = ViewModel;
        this.InitializeComponent();

        ViewModel.NavigateBack += OnNavigateBack;

        Unloaded += (s, e) =>
        {
            ViewModel.NavigateBack -= OnNavigateBack;
        };
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        var categoryDto = e.Parameter as CategoryViewModel;
        if (categoryDto != null)
        {
            ViewModel.Category = categoryDto;
        }

        base.OnNavigatedTo(e);
    }

    private void OnNavigateBack(object sender, EventArgs e)
    {
        Frame.GoBack();
    }

    private void AdminCategoryEditPage_CancelButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        Frame.GoBack();
    }
}