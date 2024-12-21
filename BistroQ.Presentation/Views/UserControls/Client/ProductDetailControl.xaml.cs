using BistroQ.Presentation.ViewModels.Models;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.Extensions;
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
        ChartViewModel = new PieChartViewModel(product.NutritionFact);
    }

    public class PieChartViewModel
    {
        public IEnumerable<ISeries> Series { get; set; }

        public PieChartViewModel(NutritionFactViewModel nutritionFact)
        {
            //Series = new[] {
            //    double.TryParse(nutritionFact?.Calories, out var calories) ? calories : 0,
            //    double.TryParse(nutritionFact?.Fiber, out var fiber) ? fiber : 0,
            //    double.TryParse(nutritionFact?.Fat, out var fat) ? fat : 0,
            //    double.TryParse(nutritionFact?.Protein, out var protein) ? protein : 0,
            //}.AsPieSeries((value, series) =>
            //{
            //    series.MaxRadialColumnWidth = 60;
            //});

            Series = new[] { 2, 4, 1, 4, 3 }.AsPieSeries((value, series) =>
    {
        series.MaxRadialColumnWidth = 60;
    });
        }
    }
}
