// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Plot.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a control that displays a <see cref="PlotModel" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Wpf
{
    using OxyPlot.Contrib.Wpf;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Markup;

    /// <summary>
    /// Represents a control that displays a <see cref="PlotModel" />.
    /// </summary>
    [ContentProperty("Series")]
    [TemplatePart(Name = PartGrid, Type = typeof(Grid))]
    public partial class Plot : PlotView, IPlot
    {
        /// <summary>
        /// The internal model.
        /// </summary>
        private PlotModel internalModel => ActualModel;

        /// <summary>
        /// The default controller.
        /// </summary>
        private readonly IPlotController defaultController;

        /// <summary>
        /// Initializes static members of the <see cref="Plot" /> class.
        /// </summary>
        static Plot()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Plot), new FrameworkPropertyMetadata(typeof(Plot)));
            PaddingProperty.OverrideMetadata(typeof(Plot), new FrameworkPropertyMetadata(new Thickness(8), AppearanceChanged));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Plot" /> class.
        /// </summary>
        public Plot()
        {
            this.series = new ObservableCollection<Series>();
            this.axes = new ObservableCollection<Axis>();
            this.annotations = new ObservableCollection<Annotation>();
            this.legends = new ObservableCollection<Legend>();

            this.series.CollectionChanged += this.OnSeriesChanged;
            this.axes.CollectionChanged += this.OnAxesChanged;
            this.annotations.CollectionChanged += this.OnAnnotationsChanged;
            this.legends.CollectionChanged += this.OnAnnotationsChanged;

            this.defaultController = new PlotController();
            var plot = new PlotModel();
            ((IPlotModel)plot).AttachPlotView(this);
            this.Model = plot;
        }

        /// <summary>
        /// Gets the annotations.
        /// </summary>
        /// <value>The annotations.</value>
        public ObservableCollection<Annotation> Annotations
        {
            get
            {
                return this.annotations;
            }
        }

        /// <summary>
        /// Gets an enumerator for logical child elements of this element.
        /// </summary>
        protected override System.Collections.IEnumerator LogicalChildren
        {
            get
            {
                foreach (var annotation in this.Annotations)
                {
                    yield return annotation;
                }

                foreach (var axis in this.Axes)
                {
                    yield return axis;
                }

                foreach (var s in this.Series)
                {
                    yield return s;
                }

                foreach (var legend in this.Legends)
                {
                    yield return legend;
                }
            }
        }

        /// <summary>
        /// Updates the model. If Model==<c>null</c>, an internal model will be created. The ActualModel.Update will be called (updates all series data).
        /// </summary>
        /// <param name="updateData">if set to <c>true</c>, all data collections will be updated.</param>
        protected virtual void UpdateModel(bool updateData = true)
        {
            this.SynchronizeProperties();
            this.SynchronizeSeries();
            this.SynchronizeAxes();
            this.SynchronizeAnnotations();
            this.SynchronizeLegends();

            if (this.ActualModel != null)
            {
                ((IPlotModel)this.ActualModel).Update(updateData);
            }
        }

        /// <summary>
        /// Called when the visual appearance is changed.
        /// </summary>
        protected void OnAppearanceChanged()
        {
            this.UpdateModel(false);
            base.InvalidatePlot(false);
        }

        /// <summary>
        /// Called when the visual appearance is changed.
        /// </summary>
        protected void OnDataChanged()
        {
            this.UpdateModel(true);
            base.InvalidatePlot(true);
        }

        void IPlot.ElementAppearanceChanged(object element)
        {
            // TODO: determine type of element to perform a more fine-grained update
            this.UpdateModel(false);
            base.InvalidatePlot(false);
        }

        void IPlot.ElementDataChanged(object element)
        {
            // TODO: determine type of element to perform a more fine-grained update
            this.UpdateModel(true);
            base.InvalidatePlot(true);
        }

        /// <summary>
        /// Called when the visual appearance is changed.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void AppearanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Plot)d).OnAppearanceChanged();
        }

        /// <summary>
        /// Called when annotations is changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs" /> instance containing the event data.</param>
        private void OnAnnotationsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.SyncLogicalTree(e);
        }

        /// <summary>
        /// Called when axes is changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs" /> instance containing the event data.</param>
        private void OnAxesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.SyncLogicalTree(e);
        }

        /// <summary>
        /// Called when series is changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs" /> instance containing the event data.</param>
        private void OnSeriesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.SyncLogicalTree(e);
        }

        /// <summary>
        /// Synchronizes the logical tree.
        /// </summary>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs" /> instance containing the event data.</param>
        private void SyncLogicalTree(NotifyCollectionChangedEventArgs e)
        {
            // In order to get DataContext and binding to work with the series, axes and annotations
            // we add the items to the logical tree
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    this.AddLogicalChild(item);
                }
            }

            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    this.RemoveLogicalChild(item);
                }
            }

            this.OnAppearanceChanged();
        }

        /// <summary>
        /// Synchronize properties in the internal Plot model
        /// </summary>
        private void SynchronizeProperties()
        {
            var m = this.internalModel;

            m.PlotType = this.PlotType;

            m.PlotMargins = this.PlotMargins.ToOxyThickness();
            m.Padding = this.Padding.ToOxyThickness();
            m.TitlePadding = this.TitlePadding;

            m.Culture = this.Culture;

            m.DefaultColors = this.DefaultColors.Select(c => c.ToOxyColor()).ToArray();
            m.DefaultFont = this.DefaultFont;
            m.DefaultFontSize = this.DefaultFontSize;

            m.Title = this.Title;
            m.TitleColor = this.TitleColor.ToOxyColor();
            m.TitleFont = this.TitleFont;
            m.TitleFontSize = this.TitleFontSize;
            m.TitleFontWeight = this.TitleFontWeight.ToOpenTypeWeight();
            m.TitleToolTip = this.TitleToolTip;

            m.Subtitle = this.Subtitle;
            m.SubtitleColor = this.SubtitleColor.ToOxyColor();
            m.SubtitleFont = this.SubtitleFont;
            m.SubtitleFontSize = this.SubtitleFontSize;
            m.SubtitleFontWeight = this.SubtitleFontWeight.ToOpenTypeWeight();

            m.TextColor = this.TextColor.ToOxyColor();
            m.SelectionColor = this.SelectionColor.ToOxyColor();

            m.RenderingDecorator = this.RenderingDecorator;

            m.AxisTierDistance = this.AxisTierDistance;

            m.PlotAreaBackground = this.PlotAreaBackground.ToOxyColor();
            m.PlotAreaBorderColor = this.PlotAreaBorderColor.ToOxyColor();
            m.PlotAreaBorderThickness = this.PlotAreaBorderThickness.ToOxyThickness();
        }

        /// <summary>
        /// Synchronizes the annotations in the internal model.
        /// </summary>
        private void SynchronizeAnnotations()
        {
            this.internalModel.Annotations.Clear();
            foreach (var a in this.Annotations)
            {
                this.internalModel.Annotations.Add(a.CreateModel());
            }
        }

        /// <summary>
        /// Synchronizes the axes in the internal model.
        /// </summary>
        private void SynchronizeAxes()
        {
            this.internalModel.Axes.Clear();
            foreach (var a in this.Axes)
            {
                this.internalModel.Axes.Add(a.CreateModel());
            }
        }

        /// <summary>
        /// Synchronizes the series in the internal model.
        /// </summary>
        private void SynchronizeSeries()
        {
            this.internalModel.Series.Clear();
            foreach (var s in this.Series)
            {
                this.internalModel.Series.Add(s.CreateModel());
            }
        }

        /// <summary>
        /// Synchronizes the legends in the internal model.
        /// </summary>
        private void SynchronizeLegends()
        {
            this.internalModel.Legends.Clear();
            foreach (var l in this.Legends)
            {
                this.internalModel.Legends.Add(l.CreateModel());
            }
        }
    }
}
