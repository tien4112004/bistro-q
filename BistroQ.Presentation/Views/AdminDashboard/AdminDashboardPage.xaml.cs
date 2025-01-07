using BistroQ.Presentation.ViewModels;
using Microsoft.UI.Xaml.Controls;


namespace BistroQ.Presentation.Views.AdminDashboard;

public sealed partial class AdminDashboardPage : Page
{
    public AdminDashboardViewModel ViewModel { get; }

    public AdminDashboardPage()
    {
        this.ViewModel = App.GetService<AdminDashboardViewModel>();
        this.InitializeComponent();
    }
}