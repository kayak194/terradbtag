using System.Windows;
using System.Windows.Controls;

namespace terradbtag.WordCloud
{
    /// <summary>
    ///     INFO: http://ikeptwalking.com/wpf-measureoverride-arrangeoverride-explained/
    ///     Responsible for arranging the given UIChildren in desired cloud shape
    /// </summary>
    public class WordCloudPanel:Panel
    {
        protected override Size MeasureOverride(Size availableSize)
        {
            var childHeight = 0.0;
            var childWidth = 0.0;
            var size = new Size(0, 0);

            foreach (UIElement child in InternalChildren)
            {
                child.Measure(new Size(availableSize.Width, availableSize.Height));

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

        protected override Size ArrangeOverride(Size finalSize)
        {
            var toggle = false;

            var yAxisHeight = 0.0;

            foreach (Button child in InternalChildren)
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