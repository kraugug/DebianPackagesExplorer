/*
 * Copyright(C) 2019, Michal Heczko All rights reserved.
 *
 * This software may be modified and distributed under the terms of the
 * GNU General Public License v3.0. See the LICENSE file for details.
 */

using DebianPackagesExplorer.Extensions;
using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace DebianPackagesExplorer.Debian
{
	public class PackageInfo
	{
		#region Properties

		public string Architecture { get; private set; }

		public string Author { get; private set; }

		[UseParseFunction(",")]
		public StringCollection Depends { get; private set; }

		public string Description { get; private set; }

		public string Filename { get; private set; }

		public string Homepage { get; private set; }

		[RealName("Installed-Size")]
		public long InstalledSize { get; private set; }

		public string Maintainer { get; private set; }

		public string MD5Sum { get; private set; }

		[RealName("Package")]
		public string Name { get; private set; }

		public string Priority { get; private set; }

		public string Section { get; private set; }

		public string SHA1 { get; private set; }

		public string SHA256 { get; private set; }

		public long Size { get; private set; }

		public string Source { get; private set; }

		[UseParseFunction("|")]
		public StringCollection Suggests { get; private set; }

		[RealName("Tag")]
		[UseParseFunction(",")]
		public StringCollection Tags { get; private set; }

		public string Version { get; private set; }

		#endregion

		#region Methods

		public static PackageInfo Parse(string str)
		{
			if (string.IsNullOrEmpty(str))
				return null;
			MatchCollection matches = Regex.Matches(str, Properties.Resources.RegExPattern_PackageInfo);
			if (matches.Count > 0)
			{
				PackageInfo info = new PackageInfo();
				PropertyInfo[] properties = info.GetType().GetProperties();
				foreach (PropertyInfo property in properties)
				{
					RealNameAttribute realName = property.GetCustomAttribute<RealNameAttribute>();
					string propertyName = (realName == null ? property.Name : realName.Name).ToUpperInvariant();
					UseParseFunctionAttribute useParseFunction = property.GetCustomAttribute<UseParseFunctionAttribute>();
					foreach (Match match in matches)
					{
						if (match.Groups["name"].Value.ToUpperInvariant().CompareTo(propertyName) == 0)
						{
							if (useParseFunction != null)
							{
								object value = property.PropertyType.GetMethod("Parse", new Type[] { typeof(string), typeof(string) }).Invoke(property, new object[] { match.Groups["value"].Value.Trim(), useParseFunction.Delimiter });
								property.SetValue(info, value, null);
							}
							else
								property.SetValue(info, Convert.ChangeType(match.Groups["value"].Value.Trim(), property.PropertyType), null);
							break;
						}
					}
				}
				return info;
			}
			return null;
		}

		#endregion
	}
}
