using DebianPackagesExplorer.Converters.Generic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;

namespace DebianPackagesExplorer.Converters
{
	public class StringCollectionToVisibilityConverter : MarkupConverter<StringCollectionToVisibilityConverter>
	{
		#region Methods

		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ((StringCollection)value)?.Count > 0 ? Visibility.Visible : Visibility.Hidden;
		}

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
