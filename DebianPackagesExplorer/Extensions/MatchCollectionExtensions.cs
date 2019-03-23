using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
