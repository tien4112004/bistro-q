using BistroQ.Presentation.ViewModels.AdminCategory;
using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Presentation.Views.AdminCategory;

public sealed partial class AdminCategoryAddPage : Page
{
    public AdminCategoryAddPageViewModel ViewModel { get; set; }

    public AdminCategoryAddPage()
    {
        ViewModel = App.GetService<AdminCategoryAddPageViewModel>();
        this.DataContext = ViewModel;
        this.InitializeComponent();

        ViewModel.NavigateBack += OnNavigateBack;

        Unloaded += (s, e) =>
        {
            ViewModel.NavigateBack -= OnNavigateBack;
        };
    }

    private void AdminCategoryAddPage_CancelButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        Frame.GoBack();
    }

    private void OnNavigateBack(object sender, EventArgs e)
    {
        Frame.GoBack();
    }

    private void Name_GettingFocus(Microsoft.UI.Xaml.UIElement sender, Microsoft.UI.Xaml.Input.GettingFocusEventArgs args)
    {
        ViewModel.Form.ResetError(nameof(ViewModel.Form.Name));
    }
}