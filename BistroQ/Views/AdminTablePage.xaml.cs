using BistroQ.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Views;

// TODO: Change the grid as appropriate for your app. Adjust the column definitions on DataGridPage.xaml.
// For more details, see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid.
public sealed partial class AdminTablePage : Page
{
    public AdminTableViewModel ViewModel
    {
        get;
    }

    public AdminTablePage()
    {
        ViewModel = App.GetService<AdminTableViewModel>();
        InitializeComponent();
    }
}
