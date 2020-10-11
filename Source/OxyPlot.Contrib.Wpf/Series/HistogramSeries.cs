// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeatMapSeries.cs" company="OxyPlot">
//   Copyright (c) 2020 OxyPlot contributors
// </copyright>
// <summary>
//   HistogramSeries WPF wrapper
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using System;
    using System.Windows;
    using System.Windows.Ink;
    using System.Windows.Media;
    using OxyPlot.Series;

    /// <summary>
    /// HistogramSeries WPF wrapper
    /// </summary>
    public class HistogramSeries : XYAxisSeries
    {
        /// <summary>
        /// Identifies the <see cref="Mapping"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MappingProperty = DependencyProperty.Register(
            "Mapping",
            typeof(Func<object, HistogramItem>),
            typeof(HistogramSeries),
            new UIPropertyMetadata(null, DataChanged));

        /// <summary>
        /// Identifies the <see cref="Stroke"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(
            "Stroke",
            typeof(Color),
            typeof(HistogramSeries),
            new PropertyMetadata(Colors.Black, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="StrokeThickness"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(
            "StrokeThickness",
            typeof(double),
            typeof(HistogramSeries),
            new PropertyMetadata(0.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="LabelFormatString"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelFormatStringProperty = DependencyProperty.Register(
            "LabelFormatString",
            typeof(string),
            typeof(HistogramSeries),
            new UIPropertyMetadata(null, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="LabelMargin"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelMarginProperty = DependencyProperty.Register(
            "LabelMargin",
            typeof(double),
            typeof(HistogramSeries),
            new PropertyMetadata(0.0, AppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="LabelPlacement"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LabelPlacementProperty = DependencyProperty.Register(
            "LabelPlacement",
            typeof(LabelPlacement),
            typeof(HistogramSeries),
            new PropertyMetadata(LabelPlacement.Outside, AppearanceChanged));

        /// <summary>
        /// Initializes static members of the <see cref="HistogramSeries"/> class.
        /// </summary>
        static HistogramSeries()
        {
            TrackerFormatStringProperty.OverrideMetadata(typeof(HistogramSeries), new PropertyMetadata(OxyPlot.Series.HistogramSeries.DefaultTrackerFormatString, AppearanceChanged));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "HistogramSeries" /> class.
        /// </summary>
        public HistogramSeries()
        {
            this.InternalSeries = new OxyPlot.Series.HistogramSeries();
        }

        /// <summary>
        /// Gets or sets the mapping.
        /// </summary>
        /// <value>The mapping.</value>
        public Func<object, HistogramItem> Mapping
        {
            get
            {
                return (Func<object, HistogramItem>)this.GetValue(MappingProperty);
            }

            set
            {
                this.SetValue(MappingProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the stroke color.
        /// </summary>
        public Color Stroke
        {
            get
            {
                return (Color)GetValue(StrokeProperty);
            }
            set
            {
                SetValue(StrokeProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        public double StrokeThickness
        {
            get
            {
                return (double)GetValue(StrokeThicknessProperty);
            }
            set
            {
                SetValue(StrokeThicknessProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the label format string.
        /// </summary>
        /// <value>The label format string.</value>
        public string LabelFormatString
        {
            get
            {
                return (string)this.GetValue(LabelFormatStringProperty);
            }

            set
            {
                this.SetValue(LabelFormatStringProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the label margin.
        /// </summary>
        /// <value>The label margin.</value>
        public double LabelMargin
        {
            get
            {
                return (double)this.GetValue(LabelMarginProperty);
            }

            set
            {
                this.SetValue(LabelMarginProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the label placement.
        /// </summary>
        /// <value>The label placement.</value>
        public LabelPlacement LabelPlacement
        {
            get
            {
                return (LabelPlacement)this.GetValue(LabelPlacementProperty);
            }

            set
            {
                this.SetValue(LabelPlacementProperty, value);
            }
        }

        /// <summary>
        /// The create model.
        /// </summary>
        /// <returns>
        /// The <see cref="Series"/>.
        /// </returns>
        public override OxyPlot.Series.Series CreateModel()
        {
            this.SynchronizeProperties(this.InternalSeries);
            return this.InternalSeries;
        }

        /// <summary>
        /// Synchronizes the properties.
        /// </summary>
        /// <param name="series">The series.</param>
        protected override void SynchronizeProperties(OxyPlot.Series.Series series)
        {
            base.SynchronizeProperties(series);
            var s = (OxyPlot.Series.HistogramSeries)series;
            s.ItemsSource = this.ItemsSource;
            s.FillColor = this.Color.ToOxyColor();
            s.Mapping = this.Mapping;
            s.StrokeColor = this.Stroke.ToOxyColor();
            s.StrokeThickness = this.StrokeThickness;
            s.LabelFormatString = this.LabelFormatString;
            s.LabelMargin = this.LabelMargin;
            s.LabelPlacement = this.LabelPlacement;
        }
    }
}