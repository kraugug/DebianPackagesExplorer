/*
 * Copyright(C) 2019, Michal Heczko All rights reserved.
 *
 * This software may be modified and distributed under the terms of the
 * GNU General Public License v3.0. See the LICENSE file for details.
 */

namespace DebianPackagesExplorer.Extensions
{
	public static class ObjectExtensions
	{
		#region Methods

		public static bool IsNumber(this object obj)
		{
			return obj is sbyte
				|| obj is byte
				|| obj is short
				|| obj is ushort
				|| obj is int
				|| obj is uint
				|| obj is long
				|| obj is ulong
				|| obj is float
				|| obj is double
				|| obj is decimal;
		}

		#endregion
	}
}
