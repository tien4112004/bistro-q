﻿using AutoMapper;
using BistroQ.Domain.Contracts.Services.Data;
using BistroQ.Presentation.Contracts.Services;
using BistroQ.Presentation.Contracts.ViewModels;
using BistroQ.Presentation.Messages;
using BistroQ.Presentation.ViewModels.AdminCategory;
using BistroQ.Presentation.ViewModels.Models;
using BistroQ.Presentation.ViewModels.States;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace BistroQ.Presentation.ViewModels;

public partial class AdminCategoryViewModel :
    ObservableRecipient,
    INavigationAware,
    IDisposable,
    IRecipient<PageSizeChangedMessage>,
    IRecipient<CurrentPageChangedMessage>
{
    private readonly IDialogService _dialogService;
    private readonly INavigationService _navigationService;
    private readonly ICategoryDataService _categoryDataService;
    private readonly IMapper _mapper;
    private readonly IMessenger _messenger;
    private bool _isDisposed = false;

    [ObservableProperty]
    private AdminCategoryState state = new();

    public IRelayCommand AddCommand { get; }
    public IRelayCommand EditCommand { get; }
    public IAsyncRelayCommand DeleteCommand { get; }
    public ICommand SortCommand { get; }
    public ICommand SearchCommand { get; }

    public AdminCategoryViewModel(
        INavigationService navigationService,
        ICategoryDataService categoryDataService,
        IDialogService dialogService,
        IMapper mapper,
        IMessenger messenger)
    {
        _navigationService = navigationService;
        _categoryDataService = categoryDataService;
        _mapper = mapper;
        _dialogService = dialogService;
        _messenger = messenger;

        State.PropertyChanged += StatePropertyChanged;

        AddCommand = new RelayCommand(NavigateToAddPage);
        EditCommand = new RelayCommand(NavigateToEditPage, () => State.CanEdit);
        DeleteCommand = new AsyncRelayCommand(DeleteSelectedCategoryAsync, () => State.CanDelete);
        SortCommand = new RelayCommand<(string column, string direction)>(ExecuteSortCommand);
        SearchCommand = new RelayCommand(ExecuteSearchCommand);

        _messenger.RegisterAll(this);
    }

    private void StatePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(State.SelectedCategory))
        {
            EditCommand.NotifyCanExecuteChanged();
            DeleteCommand.NotifyCanExecuteChanged();
        }
    }

    public async Task OnNavigatedTo(object parameter)
    {
        await LoadDataAsync();
    }

    public Task OnNavigatedFrom()
    {
        Dispose();
        return Task.CompletedTask;
    }

    private async Task LoadDataAsync()
    {
        if (State.IsLoading || _isDisposed) return;

        try
        {
            State.IsLoading = true;
            var result = await _categoryDataService.GetCategoriesAsync(State.Query);

            var categories = _mapper.Map<IEnumerable<CategoryViewModel>>(result.Data);
            State.Source = new ObservableCollection<CategoryViewModel>(categories);
            _messenger.Send(new PaginationChangedMessage(
                result.TotalItems,
                result.CurrentPage,
                result.TotalPages
            ));
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorDialog(ex.Message, "Error");
        }
        finally
        {
            State.IsLoading = false;
        }
    }

    private void NavigateToAddPage() =>
        _navigationService.NavigateTo(typeof(AdminCategoryAddPageViewModel).FullName);

    private void NavigateToEditPage()
    {
        if (State.SelectedCategory?.CategoryId != null)
        {
            _navigationService.NavigateTo(typeof(AdminCategoryEditPageViewModel).FullName, State.SelectedCategory);
        }
    }

    private async Task DeleteSelectedCategoryAsync()
    {
        if (State.SelectedCategory?.CategoryId == null) return;

        try
        {
            var result = await _dialogService.ShowConfirmDeleteDialog();
            if (result != ContentDialogResult.Primary) return;

            var success = await _categoryDataService.DeleteCategoryAsync(State.SelectedCategory.CategoryId.Value);
            if (success)
            {
                await _dialogService.ShowSuccessDialog("Category deleted successfully.", "Success");
                State.Source.Remove(State.SelectedCategory);
                State.SelectedCategory = null;
                await LoadDataAsync();
            }
            else
            {
                await _dialogService.ShowErrorDialog("Failed to delete category.", "Error");
            }
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorDialog(ex.Message, "Error");
        }
    }

    private void ExecuteSortCommand((string column, string direction) sortParams)
    {
        var (column, direction) = sortParams;
        State.Query.OrderBy = column;
        State.Query.OrderDirection = direction;
        State.ReturnToFirstPage();
        _ = LoadDataAsync();
    }

    public void AdminCategoryDataGrid_Sorting(object sender, DataGridColumnEventArgs e)
    {
        var column = e.Column;
        var sortDirection = column.SortDirection == null || column.SortDirection == DataGridSortDirection.Descending
            ? "asc"
            : "des";

        column.SortDirection = sortDirection == "asc"
            ? DataGridSortDirection.Ascending
            : DataGridSortDirection.Descending;

        var sortParams = (column.Tag.ToString(), sortDirection);
        SortCommand.Execute(sortParams);
    }

    private void ExecuteSearchCommand()
    {
        State.ReturnToFirstPage();
        _ = LoadDataAsync();
    }

    public void Dispose()
    {
        if (_isDisposed) return;
        _isDisposed = true;
        if (State != null)
        {
            State.PropertyChanged -= StatePropertyChanged;
        }

        _messenger.UnregisterAll(this);
    }

    public void Receive(PageSizeChangedMessage message)
    {
        State.Query.Size = message.NewPageSize;
        State.ReturnToFirstPage();
        _ = LoadDataAsync();
    }

    public void Receive(CurrentPageChangedMessage message)
    {
        State.Query.Page = message.NewCurrentPage;
        _ = LoadDataAsync();
    }
}
