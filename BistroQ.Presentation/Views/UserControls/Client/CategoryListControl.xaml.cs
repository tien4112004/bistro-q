using BistroQ.Presentation.ViewModels.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Presentation.Views.UserControls.Client
{
    public sealed partial class CategoryListControl : UserControl
    {
        public static readonly DependencyProperty CategoriesProperty =
            DependencyProperty.Register(
                nameof(Categories),
                typeof(IEnumerable<CategoryViewModel>),
                typeof(CategoryListControl),
                new PropertyMetadata(null));

        public static readonly DependencyProperty SelectedCategoryProperty =
            DependencyProperty.Register(
                nameof(SelectedCategory),
                typeof(CategoryViewModel),
                typeof(CategoryListControl),
                new PropertyMetadata(null));

        public static readonly DependencyProperty IsLoadingCategoryProperty =
            DependencyProperty.Register(
                nameof(IsLoadingCategory),
                typeof(bool),
                typeof(CategoryListControl),
                new PropertyMetadata(false));

        public event EventHandler CategoryChanged;

        public IEnumerable<CategoryViewModel> Categories
        {
            get => (IEnumerable<CategoryViewModel>)GetValue(CategoriesProperty);
            set => SetValue(CategoriesProperty, value);
        }

        public CategoryViewModel SelectedCategory
        {
            get => (CategoryViewModel)GetValue(SelectedCategoryProperty);
            set => SetValue(SelectedCategoryProperty, value);
        }

        public bool IsLoadingCategory
        {
            get => (bool)GetValue(IsLoadingCategoryProperty);
            set => SetValue(IsLoadingCategoryProperty, value);
        }

        public CategoryListControl()
        {
            this.InitializeComponent();
        }
    }
}