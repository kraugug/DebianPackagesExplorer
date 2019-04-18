/*
 * Copyright(C) 2019, Michal Heczko All rights reserved.
 *
 * This software may be modified and distributed under the terms of the
 * GNU General Public License v3.0. See the LICENSE file for details.
 */

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
