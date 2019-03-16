/*
 * Copyright(C) 2018, Michal Heczko All rights reserved.
 *
 * This software may be modified and distributed under the terms of the
 * GNU General Public License v3.0. See the LICENSE file for details.
 */

using System;
using System.Globalization;

namespace DebianPackagesExplorer.Converters.Generic
{
	public class BoolToValueConverter<T> : MarkupConverter<BoolToValueConverter<T>>
	{
		#region Properties

		public T FalseValue { get; set; }

		public bool Inverted { get; set; }

		public T TrueValue { get; set; }

		#endregion

		#region Methods

		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (Inverted)
				return System.Convert.ToBoolean(value) ? FalseValue : TrueValue;
			return System.Convert.ToBoolean(value) ? TrueValue : FalseValue;
		}

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Inverted ? value.Equals(FalseValue) : value.Equals(TrueValue);
		}

		#endregion

		#region Constructors

		public BoolToValueConverter()
		{
			FalseValue = default(T);
			TrueValue = default(T);
		}

		#endregion
	}
}
