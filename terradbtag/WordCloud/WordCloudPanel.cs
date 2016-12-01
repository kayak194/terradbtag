using System.Windows;
using System.Windows.Controls;
using WordCloudCalculator.Contract;
using WordCloudCalculator.ExtractingWordCloudCalculator;
using WordCloudCalculator.WordCloudCalculator;
using WordCloudCalculator.Contract.Visualization;
using customSize = WordCloudCalculator.Contract.Visualization.Size;

namespace terradbtag.WordCloud
{
    /// <summary>
    ///     INFO: http://ikeptwalking.com/wpf-measureoverride-arrangeoverride-explained/
    ///     Responsible for arranging the given UIChildren in desired cloud shape
    /// </summary>
    public class WordCloudPanel:Panel
    {
        public static customSize GetTextMetrics(string text, double size) => new customSize(text.Length, 1);

        protected override System.Windows.Size MeasureOverride(System.Windows.Size availableSize)
        {
            var childHeight = 0.0;
            var childWidth = 0.0;
            var size = new System.Windows.Size(0, 0);

            foreach (UIElement child in InternalChildren)
            {
                child.Measure(new System.Windows.Size(availableSize.Width, availableSize.Height));

                if (child.DesiredSize.Width > childWidth)
                {
                    childWidth = child.DesiredSize.Width;   //We will be stacking vertically.
                }
                childHeight += child.DesiredSize.Height;    //Total height needs to be summed up.
            }

            size.Width = double.IsPositiveInfinity(availableSize.Width) ? childWidth : availableSize.Width;
            size.Height = double.IsPositiveInfinity(availableSize.Height) ? childHeight : availableSize.Height;

            return size;
        }

        protected override System.Windows.Size ArrangeOverride(System.Windows.Size finalSize)
        {

            var calc = new ExtractingWordCloudCalculator<SimpleAppearenceCalculationMethod>();

            var appearenaceArgs = new WordCloudAppearenceArguments()
            {
                PanelSize = new WordCloudCalculator.Contract.Visualization.Size(finalSize.Width, finalSize.Height),
                FontSizeRange = new Range(0.0, 15.0),
                OpacityRange = new Range(0.5, 1.0),
                WordMargin = WordCloudCalculator.Contract.Visualization.Margin.None,
                WordSizeCalculator = GetTextMetrics
            };

            var toggle = false;

            var yAxisHeight = 0.0;

            foreach (UIElement child in InternalChildren)
            {
                if (toggle == false)
                {
                    var rec = new Rect(new Point(0, yAxisHeight), child.DesiredSize);
                    child.Arrange(rec);
                    toggle = true;
                }
                else
                {
                    yAxisHeight += child.DesiredSize.Height;
                    var rec = new Rect(new Point(0, finalSize.Height - yAxisHeight), child.DesiredSize);
                    child.Arrange(rec);
                    toggle = false;
                }
            }
            return finalSize;
        }
    }
}