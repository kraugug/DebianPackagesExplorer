/*
 * Copyright(C) 2019, Michal Heczko All rights reserved.
 *
 * This software may be modified and distributed under the terms of the
 * GNU General Public License v3.0. See the LICENSE file for details.
 */

using System.Text.RegularExpressions;

namespace DebianPackagesExplorer.Debian
{
	public class SelectedSourcePackageInfo
	{
		#region Fields

		private string m_BaseUrl;

		#endregion

		#region Properties

		public string Architecture { get; }

		public string BaseUrl { get { return m_BaseUrl; } }

		public string CodeName { get; }

		public string Component { get; }

		public string FileName { get; }

		public string Url { get; }

		#endregion

		#region Methods

		public static SelectedSourcePackageInfo Parse(string str)
		{
			Match match = Regex.Match(str, Properties.Resources.RegEx_Pattern_SourcePackageInfoByLink);
			if (match.Success)
				return new SelectedSourcePackageInfo(
					match.Groups[Properties.Resources.RegEx_GroupName_Architecture].Value,
					match.Groups[Properties.Resources.RegEx_GroupName_BaseUrl].Value,
					match.Groups[Properties.Resources.RegEx_GroupName_CodeName].Value,
					match.Groups[Properties.Resources.RegEx_GroupName_Component].Value,
					match.Groups[Properties.Resources.RegEx_GroupName_FileName].Value);
			return null;
		}

		#endregion

		#region Constructors

		public SelectedSourcePackageInfo(ComponentInfo componentInfo)
		{
			Architecture = componentInfo.Parent.Name;
			CodeName = componentInfo.Parent.Parent.Name;
			Component = componentInfo.Name;
			Url = componentInfo.Url;
			m_BaseUrl = Url.Substring(0, Url.IndexOf("dists"));
		}

		private SelectedSourcePackageInfo(string architecture, string baseUrl, string codeName, string component, string fileName)
		{
			Architecture = architecture;
			m_BaseUrl = baseUrl;
			CodeName = codeName;
			Component = component;
			FileName = fileName;
			Url = string.Format("{0}/dists/{1}/{2}/binary-{3}/{4}", baseUrl, codeName, component, architecture, fileName);
		}

		#endregion
	}
}
