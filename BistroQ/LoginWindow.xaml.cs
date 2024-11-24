using BistroQ.Views;
using Microsoft.UI.Xaml;

namespace BistroQ;

public sealed partial class LoginWindow : Window
{
    public LoginWindow()
    {
        this.InitializeComponent();
        this.Content = new LoginPage(this);
    }
}
