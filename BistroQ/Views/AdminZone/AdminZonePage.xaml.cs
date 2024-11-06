using BistroQ.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Views;

// TODO: Change the grid as appropriate for your app. Adjust the column definitions on DataGridPage.xaml.
// For more details, see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid.
public sealed partial class AdminZonePage : Page
{
    public AdminZoneViewModel ViewModel { get; }

    public AdminZonePage()
    {
        ViewModel = App.GetService<AdminZoneViewModel>();
        this.DataContext = ViewModel;
        InitializeComponent();
    }

    private void Control2_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        if (args.ChosenSuggestion != null)
        {
            ViewModel.SearchText = args.ChosenSuggestion.ToString();
        }
        else
        {
            ViewModel.SearchText = args.QueryText;
        }

        ViewModel.SearchCommand.Execute(null);
    }

    private void Control2_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        sender.Text = args.SelectedItem.ToString();
    }
}
