using BistroQ.ViewModels.Client;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BistroQ.Views.Client
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public HomePageViewModel ViewModel { get; }

        public HomePage()
        {
            this.ViewModel = App.GetService<HomePageViewModel>();
            this.InitializeComponent();
        }
    }
}
