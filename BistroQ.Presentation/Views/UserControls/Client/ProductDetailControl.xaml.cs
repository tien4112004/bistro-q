using BistroQ.Presentation.ViewModels.Models;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Microsoft.UI.Xaml.Controls;

namespace BistroQ.Presentation.Views.UserControls.Client;

public sealed partial class ProductDetailControl : UserControl
{
    public ProductViewModel ViewModel { get; set; }

    public PieChartViewModel ChartViewModel { get; set; }

    public ProductDetailControl(ProductViewModel product)
    {
        this.InitializeComponent();
        ViewModel = product;
        DataContext = this;
        ChartViewModel = new PieChartViewModel(product.NutritionFact);
        //Chart.Series = ChartViewModel.Series;

        var series = ChartViewModel.Series;
        System.Diagnostics.Debug.WriteLine($"Series count: {series.Count()}");
        foreach (var s in series)
        {
            System.Diagnostics.Debug.WriteLine($"Values: {string.Join(", ", (s as PieSeries<double>)?.Values ?? Array.Empty<double>())}");
        }
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
