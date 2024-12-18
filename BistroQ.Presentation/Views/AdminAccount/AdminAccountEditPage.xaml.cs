using BistroQ.Presentation.ViewModels.AdminAccount;
using BistroQ.Presentation.ViewModels.Models;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace BistroQ.Presentation.Views.AdminAccount;

public sealed partial class AdminAccountEditPage : Page
{
    public AdminAccountEditPageViewModel ViewModel { get; set; }

    public AdminAccountEditPage()
    {
        InitializeComponent();
        ViewModel = App.GetService<AdminAccountEditPageViewModel>();
        this.DataContext = ViewModel;

        this.Loaded += AdminAccountEditPage_Loaded;
        ViewModel.NavigateBack += OnNavigateBack;

        this.Unloaded += (s, e) =>
        {
            ViewModel.NavigateBack -= OnNavigateBack;
        };
    }

    private async void AdminAccountEditPage_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        await ViewModel.LoadZonesAsync();
        if (ViewModel.SelectedZoneId.HasValue)
        {
            await ViewModel.LoadTablesAsync(ViewModel.SelectedZoneId.Value);
        }
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        var accountDto = e.Parameter as AccountViewModel;
        if (accountDto != null)
        {
            ViewModel.Account = accountDto;
        }

        base.OnNavigatedTo(e);
    }

    private void AdminAccountEditPage_CancelButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        Frame.GoBack();
    }

    private void OnNavigateBack(object sender, EventArgs e)
    {
        Frame.GoBack();
    }
}