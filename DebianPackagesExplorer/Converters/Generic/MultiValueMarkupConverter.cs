/*
 * Copyright(C) 2019, Michal Heczko All rights reserved.
 *
 * This software may be modified and distributed under the terms of the
 * GNU General Public License v3.0. See the LICENSE file for details.
 */

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace DebianPackagesExplorer.Converters.Generic
{
	public abstract class MultiValueMarkupConverter<T> : MarkupExtension, IMultiValueConverter where T : class, new()
	{
		#region Methods

		public abstract object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);

		public abstract object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture);

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return this;
		}

		#endregion
	}
}
