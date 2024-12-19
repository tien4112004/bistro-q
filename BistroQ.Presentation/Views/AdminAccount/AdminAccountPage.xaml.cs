using BistroQ.Presentation.Helpers;
using BistroQ.Presentation.ViewModels;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;

namespace BistroQ.Presentation.Views;

public sealed partial class AdminAccountPage : Page
{
    public AdminAccountViewModel ViewModel
    {
        get;
    }

    public AdminAccountPage()
    {
        ViewModel = App.GetService<AdminAccountViewModel>();
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

    private void ViewModel_AdminAccountDataGrid_Sorting(object? sender, DataGridColumnEventArgs e)
    {
        ViewModel.AdminAccountDataGrid_Sorting(sender, e);
    }
    private void Button_PointerEntered(object sender, PointerRoutedEventArgs e)
    {
        (sender as UIElement)?.ChangeCursor(CursorType.Hand);
    }

    private void Button_PointerExited(object sender, PointerRoutedEventArgs e)
    {
        (sender as UIElement)?.ChangeCursor(CursorType.Arrow);
    }
}