using BistroQ.Presentation.ViewModels.AdminTable;
using BistroQ.Presentation.ViewModels.Models;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace BistroQ.Presentation.Views.AdminTable;

public sealed partial class AdminTableEditPage : Page
{
    public AdminTableEditPageViewModel ViewModel { get; set; }

    public AdminTableEditPage()
    {
        InitializeComponent();
        ViewModel = App.GetService<AdminTableEditPageViewModel>();
        this.DataContext = ViewModel;

        this.Loaded += AdminTableEditPage_Loaded;
        ViewModel.NavigateBack += OnNavigateBack;

        this.Unloaded += (s, e) =>
        {
            ViewModel.NavigateBack -= OnNavigateBack;
        };
    }

    private async void AdminTableEditPage_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        await ViewModel.LoadZonesAsync();
        TableEditPage_ZoneComboBox.SelectedValue = ViewModel.Request.ZoneId;
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        var tableDto = e.Parameter as TableViewModel;
        if (tableDto != null)
        {
            ViewModel.Table = tableDto;
        }

        base.OnNavigatedTo(e);
    }

    private void AdminTableEditPage_CancelButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        Frame.GoBack();
    }

    private void OnNavigateBack(object sender, EventArgs e)
    {
        Frame.GoBack();
    }
}