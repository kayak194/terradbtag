using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using WordCloudCalculator.Contract;
using WordCloudCalculator.Contract.Visualization;
using WordCloudCalculator.WordCloudCalculator;
using Size = WordCloudCalculator.Contract.Visualization.Size;

namespace terradbtag.WordCloud
{
	class TerraDbWordCloudAppearenceArguments : IWordCloudAppearenceArguments
	{
		public Size PanelSize { get; set; }
		public Range FontSizeRange { get; set; } = new Range(12, 32);
		public Range OpacityRange { get; set; } = new Range(0.4, 1);
		public Margin WordMargin { get; set; } = Margin.None;
		public Func<string, double, Size> WordSizeCalculator { get; set; } = WordSizeCalculationMethod;
		private static Size WordSizeCalculationMethod(string s, double d)
		{
			var formattedText = new FormattedText(s, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight, new Typeface("Segoe UI"), d, Brushes.Black);
			return new Size(formattedText.Width + 12, formattedText.Height + 12);
		}
	}
}