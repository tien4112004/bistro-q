using BistroQ.Presentation.ViewModels.Models;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.WinUI;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Presentation.Views.UserControls.Client;

public sealed partial class ProductDetailControl : UserControl
{
    public ProductViewModel ViewModel { get; set; }

    public PieChartViewModel ChartViewModel { get; set; }

    private readonly DispatcherQueue dispatcherQueue;

    public ProductDetailControl(ProductViewModel product)
    {
        this.InitializeComponent();
        dispatcherQueue = DispatcherQueue.GetForCurrentThread();

        ViewModel = product;
        ChartViewModel = new PieChartViewModel(product.NutritionFact);

        this.Loaded += UserControl_Loaded;
    }

    private async void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        dispatcherQueue.TryEnqueue(() =>
        {
            AddChart();
        });
    }

    private void AddChart()
    {
        var pieChart = new PieChart
        {
            Series = ChartViewModel.Series,
            Width = 216,
            Height = 216,
            Margin = new Thickness(8),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };
        ChartContainer.Child = pieChart;
    }

    public class PieChartViewModel
    {
        public IEnumerable<ISeries> Series { get; set; }
        public PieChartViewModel(NutritionFactViewModel nutritionFact)
        {
            Series = new ISeries[]
            {
                new PieSeries<double> { Values = new double[] { double.TryParse(nutritionFact?.Calories, out var calories) ? calories : 0 }, Name = "Calories" },
                new PieSeries<double> { Values = new double[] { double.TryParse(nutritionFact?.Fat, out var fat) ? fat : 0 }, Name = "Fat" },
                new PieSeries<double> { Values = new double[] { double.TryParse(nutritionFact?.Fiber, out var fiber) ? fiber : 0 }, Name = "Fiber" },
                new PieSeries<double> { Values = new double[] { double.TryParse(nutritionFact?.Protein, out var protein) ? protein : 0 }, Name = "Protein" }
            };
        }
    }
}
