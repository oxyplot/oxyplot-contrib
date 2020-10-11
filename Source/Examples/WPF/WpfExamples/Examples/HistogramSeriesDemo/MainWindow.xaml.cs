// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.ObjectModel;
using System.Windows;
using OxyPlot;

namespace HistogramSeriesDemo
{
    using OxyPlot.Axes;
    using OxyPlot.Legends;
    using OxyPlot.Series;
    using System;
    using System.Linq;
    using WpfExamples;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Example("Histogram series.")]
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            var rnd = new Random(1);

            // Create some data
            double sample()
            {
                var x = rnd.NextDouble();
                var y = rnd.NextDouble();
                return Math.Sqrt(x * x + y * y);
            }

            var samples = Enumerable.Range(0, 100000).Select(_ => sample());
            var bins = HistogramHelpers.CreateUniformBins(0, 1.5, 15);
            var binningOptions = new BinningOptions(BinningOutlierMode.RejectOutliers, BinningIntervalType.InclusiveLowerBound, BinningExtremeValueMode.IncludeExtremeValues);
            this.Items = new Collection<HistogramItem>(HistogramHelpers.Collect(samples, bins, binningOptions).ToList());

            foreach (var item in this.Items)
            {
                item.Color = item.RangeEnd <= 1 ? OxyColors.Orange : OxyColors.Blue ;
            }

            // Just for fun
            var piApproximation = 4.0 * this.Items.Where(item => item.RangeEnd <= 1).Sum(item => item.Count) / this.Items.Sum(item => item.Count);

            // Create the plot model
            var tmp = new PlotModel { Title = "Histogram Series" };

            // Add the axes, note that MinimumPadding and AbsoluteMinimum should be set on the value axis.
            tmp.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Frequency" });
            tmp.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Value" });

            // Add the series, note that the BarSeries are using the same ItemsSource as the CategoryAxis.
            tmp.Series.Add(new HistogramSeries() { ItemsSource = this.Items, StrokeThickness = 1 });

            this.Model1 = tmp;

            this.DataContext = this;
        }

        public Collection<HistogramItem> Items { get; set; }

        public PlotModel Model1 { get; set; }
    }
}