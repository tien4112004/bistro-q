using BistroQ.Presentation.ViewModels;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Presentation.Views;

public sealed partial class AdminCategoryPage : Page
{
    public AdminCategoryViewModel ViewModel { get; }

    public AdminCategoryPage()
    {
        ViewModel = App.GetService<AdminCategoryViewModel>();
        this.DataContext = ViewModel;
        InitializeComponent();
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

    private void ViewModel_AdminCategoryDataGrid_Sorting(object? sender, DataGridColumnEventArgs e)
    {
        ViewModel.AdminCategoryDataGrid_Sorting(sender, e);
    }
}