using BistroQ.Presentation.ViewModels;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Presentation.Views;

public sealed partial class AdminZonePage : Page
{
    public AdminZoneViewModel ViewModel { get; }

    public AdminZonePage()
    {
        ViewModel = App.GetService<AdminZoneViewModel>();
        this.DataContext = ViewModel;
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

    private void ViewModel_AdminZoneDataGrid_Sorting(object? sender, DataGridColumnEventArgs e)
    {
        ViewModel.AdminZoneDataGrid_Sorting(sender, e);
    }
}