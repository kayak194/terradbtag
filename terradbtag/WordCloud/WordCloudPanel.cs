using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using WordCloudCalculator.Contract;
using WordCloudCalculator.Contract.Word;
using WordCloudCalculator.ExtractingWordCloudCalculator;
using WordCloudCalculator.WordCloudCalculator;
using VisSize = WordCloudCalculator.Contract.Visualization.Size;

namespace terradbtag.WordCloud
{
    /// <summary>
    ///     INFO: http://ikeptwalking.com/wpf-measureoverride-arrangeoverride-explained/
    ///     Responsible for arranging the given UIChildren in desired cloud shape
    /// </summary>
    public class WordCloudPanel:Canvas
    {
        public static VisSize GetTextMetrics(string text, double size) => new VisSize(text.Length, 1);

        protected override Size MeasureOverride(System.Windows.Size availableSize)
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
            foreach (ContentPresenter child in InternalChildren)
            {
	            var visWord = child.Content as VisualizedWord;
	            if (visWord == null) continue;
				var rec = new Rect(new Point(visWord.Position.Left,visWord.Position.Top), new Size(visWord.Size.Width, visWord.Size.Height));
				child.Arrange(rec);
            }
            return finalSize;
        }
    }
}