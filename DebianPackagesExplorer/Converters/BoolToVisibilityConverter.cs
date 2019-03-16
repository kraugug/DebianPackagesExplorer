/*
 * Copyright(C) 2018, Michal Heczko All rights reserved.
 *
 * This software may be modified and distributed under the terms of the
 * GNU General Public License v3.0. See the LICENSE file for details.
 */

using DebianPackagesExplorer.Converters.Generic;
using System.Windows;

namespace DebianPackagesExplorer.Converters
{
	public class BoolToVisibilityConverter : BoolToValueConverter<Visibility>
	{
		#region Constructors

		public BoolToVisibilityConverter()
		{
			FalseValue = Visibility.Collapsed;
			TrueValue = Visibility.Visible;
		}

		#endregion
	}
}
