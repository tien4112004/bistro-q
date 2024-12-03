using BistroQ.Presentation.Views;
using Microsoft.UI.Xaml;

namespace BistroQ.Presentation;

public sealed partial class LoginWindow : Window
{
    public LoginWindow()
    {
        this.InitializeComponent();
        this.Content = new LoginPage(this);
    }
}
