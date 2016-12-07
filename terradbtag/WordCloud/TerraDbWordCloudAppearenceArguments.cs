using System;
using System.Windows;
using WordCloudCalculator.Contract;
using WordCloudCalculator.WordCloudCalculator;
using WordCloudCalculator.WPF;

namespace terradbtag.WordCloud
{
	class TerraDbWordCloudAppearenceArguments : IWordCloudAppearenceArguments
	{
		public TerraDbWordCloudAppearenceArguments()
		{
			WordSizeCalculator = WordSizeCalculatorFactory.FormattedTextWordSizeCalculator();
		}
		public Size PanelSize { get; set; }
		public Range FontSizeRange { get; set; } = new Range(9, 20);
		public Range OpacityRange { get; set; } = new Range(0.6, 1);
		public Func<string, double, Size> WordSizeCalculator { get; set; }
	}
}