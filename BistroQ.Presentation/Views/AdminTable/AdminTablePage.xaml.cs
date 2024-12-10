using BistroQ.Presentation.ViewModels;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Presentation.Views;

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
        this.Unloaded += (s, e) => ViewModel.Dispose();
    }

    private void Control2_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        if (args.ChosenSuggestion != null)
        {
            ViewModel.State.SearchText = args.ChosenSuggestion.ToString();
        }
        else
        {
            ViewModel.State.SearchText = args.QueryText;
        }

        ViewModel.SearchCommand.Execute(null);
    }

    private void Control2_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        sender.Text = args.SelectedItem.ToString();
    }

    private void ViewModel_AdminTableDataGrid_Sorting(object? sender, DataGridColumnEventArgs e)
    {
        ViewModel.AdminTableDataGrid_Sorting(sender, e);
    }
}