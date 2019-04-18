/*
 * Copyright(C) 2019, Michal Heczko All rights reserved.
 *
 * This software may be modified and distributed under the terms of the
 * GNU General Public License v3.0. See the LICENSE file for details.
 */

using System.Text.RegularExpressions;

namespace DebianPackagesExplorer.Extensions
{
    public static class MatchCollectionExtensions
    {
		#region Methods

		public static string GetByGroupName(this MatchCollection collection, string name)
		{
			foreach (Match item in collection)
				if (item.Groups["name"].Value.ToLower().CompareTo(name) == 0)
					return item.Groups["value"].Value;
			return null;
		}

		#endregion
	}
}
