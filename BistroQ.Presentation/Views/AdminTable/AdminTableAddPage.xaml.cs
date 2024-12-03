using BistroQ.Presentation.ViewModels.AdminTable;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace BistroQ.Presentation.Views.AdminTable;

public sealed partial class AdminTableAddPage : Page
{
    public AdminTableAddPageViewModel ViewModel { get; set; }

    public AdminTableAddPage()
    {
        this.InitializeComponent();
        ViewModel = App.GetService<AdminTableAddPageViewModel>();
        this.DataContext = ViewModel;
        
        this.Loaded += AdminTableAddPage_Loaded;
        
        ViewModel.NavigateBack += OnNavigateBack;

        Unloaded += (s, e) =>
        {
            ViewModel.NavigateBack -= OnNavigateBack;
        };
    }
    
    private async void AdminTableAddPage_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        await ViewModel.LoadZonesAsync();
    }

    private void AdminTableAddPage_CancelButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
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