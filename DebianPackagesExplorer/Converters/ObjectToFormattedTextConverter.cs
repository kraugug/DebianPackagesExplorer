using DebianPackagesExplorer.Converters.Generic;
using System;
using System.Globalization;

namespace DebianPackagesExplorer.Converters
{
	public class ObjectToFormattedTextConverter : MarkupConverter<ObjectToFormattedTextConverter>
	{
		#region Properties

		public string Format { get; set; }

		#endregion

		#region Methods

		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return string.Format(Format, value);
		}

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region Constructors

		public ObjectToFormattedTextConverter()
		{
			Format = "{0}";
		}

		#endregion
	}
}
