/*
 * Copyright(C) 2019, Michal Heczko All rights reserved.
 *
 * This software may be modified and distributed under the terms of the
 * GNU General Public License v3.0. See the LICENSE file for details.
 */

using DebianPackagesExplorer.Converters.Generic;
using System;
using System.Globalization;
using System.Linq;

namespace DebianPackagesExplorer.Converters
{
	public class MultiValueEqualityConverter : MultiValueMarkupConverter<MultiValueEqualityConverter>
	{
		#region Methods

		public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			return values?.All(o => o?.Equals(values[0]) == true) == true || values?.All(o => o == null) == true;
		}

		public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
