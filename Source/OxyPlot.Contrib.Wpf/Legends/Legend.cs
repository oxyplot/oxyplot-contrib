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
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Markup;

    /// <summary>
    /// Represents a control that displays a <see cref="OxyPlot.Legends.Legend" />.
    /// </summary>
    public partial class Legend : FrameworkElement
    {
        /// <summary>
        /// The internal model.
        /// </summary>
        private readonly OxyPlot.Legends.Legend internalLegend;

        /// <summary>
        /// Initializes static members of the <see cref="Legend" /> class.
        /// </summary>
        public Legend()
        {
            this.internalLegend = new OxyPlot.Legends.Legend();
        }

        /// <summary>
        /// Synchronize properties in the internal Plot model
        /// </summary>
        private void SynchronizeProperties()
        {
            var m = this.internalLegend;

            m.LegendTextColor = this.LegendTextColor.ToOxyColor();
            m.LegendTitle = this.LegendTitle;
            m.LegendTitleColor = this.LegendTitleColor.ToOxyColor();
            m.LegendTitleFont = this.LegendTitleFont;
            m.LegendTitleFontSize = this.LegendTitleFontSize;
            m.LegendTitleFontWeight = this.LegendTitleFontWeight.ToOpenTypeWeight();
            m.LegendFont = this.LegendFont;
            m.LegendFontSize = this.LegendFontSize;
            m.LegendFontWeight = this.LegendFontWeight.ToOpenTypeWeight();
            m.LegendSymbolLength = this.LegendSymbolLength;
            m.LegendSymbolMargin = this.LegendSymbolMargin;
            m.LegendPadding = this.LegendPadding;
            m.LegendColumnSpacing = this.LegendColumnSpacing;
            m.LegendItemSpacing = this.LegendItemSpacing;
            m.LegendLineSpacing = this.LegendLineSpacing;
            m.LegendMargin = this.LegendMargin;
            m.LegendMaxHeight = this.LegendMaxHeight;
            m.LegendMaxWidth = this.LegendMaxWidth;

            m.LegendBackground = this.LegendBackground.ToOxyColor();
            m.LegendBorder = this.LegendBorder.ToOxyColor();
            m.LegendBorderThickness = this.LegendBorderThickness;

            m.LegendPlacement = this.LegendPlacement;
            m.LegendPosition = this.LegendPosition;
            m.LegendOrientation = this.LegendOrientation;
            m.LegendItemOrder = this.LegendItemOrder;
            m.LegendItemAlignment = this.LegendItemAlignment.ToHorizontalAlignment();
            m.LegendSymbolPlacement = this.LegendSymbolPlacement;

            m.IsLegendVisible = this.IsLegendVisible;
        }

        /// <summary>
        /// Called when the visual appearance is changed.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs" /> instance containing the event data.</param>
        private static void AppearanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Legend)d).OnVisualChanged();
        }

        /// <summary>
        /// Handles changed visuals.
        /// </summary>
        protected void OnVisualChanged()
        {
            var pc = this.Parent as IPlotView;
            if (pc != null)
            {
                pc.InvalidatePlot(false);
            }
        }
    }
}
