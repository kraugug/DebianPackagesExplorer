﻿/*
 * Copyright(C) 2019, Michal Heczko All rights reserved.
 *
 * This software may be modified and distributed under the terms of the
 * GNU General Public License v3.0. See the LICENSE file for details.
 */

using DebianPackagesExplorer.Converters.Generic;
using System;
using System.Globalization;
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
