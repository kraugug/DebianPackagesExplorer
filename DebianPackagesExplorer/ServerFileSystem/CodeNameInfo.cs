/*
 * Copyright(C) 2019, Michal Heczko All rights reserved.
 *
 * This software may be modified and distributed under the terms of the
 * GNU General Public License v3.0. See the LICENSE file for details.
 */

using DebianPackagesExplorer.Extensions;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace DebianPackagesExplorer.ServerFileSystem
{
	public class CodeNameInfo : FolderInfo
	{
		#region Properties

		public DateTime Date { get; }

		public string Description { get; }

		public string FullName {  get { return ToString(); } }

		public bool IsInDevelopment { get; }

		public string Label { get; }

		public string Origin { get; }

		public string Suite { get; }

		public string Version { get; }

		#endregion

		#region Methods

		public static CodeNameInfo Parse(Match regexMatch, string baseUrl)
		{
			string name = regexMatch.Groups[nameof(Name).ToLower()].Value.Trim();
			baseUrl = string.Format(baseUrl.EndsWith("/") ? "{0}dists/{1}" : "{0}/dists/{1}", baseUrl, name);
			try
			{
				return new CodeNameInfo(baseUrl);
			}
			catch
			{
				return null;
			}
		}

		public override string ToString()
		{
			if (IsInDevelopment)
				return string.Format("{0} {1} - {2} (In development)", Label, Version, Name);
			return string.Format("{0} {1} - {2}", Label, Version, Name);
		}

		#endregion

		#region  Constructor

		public CodeNameInfo(string baseUrl, bool isInDevelopment = false) : base(null, baseUrl)
		{
			WebRequest webRequest = WebRequest.Create(string.Format("{0}/Release", BaseUrl));
			{
				using (Stream stream = webRequest.GetResponse().GetResponseStream())
				using (StreamReader reader = new StreamReader(stream))
				{
					MatchCollection matches = Regex.Matches(reader.ReadToEnd(), Properties.Resources.RegEx_Pattern_ReleaseFile);
					if (matches.Count > 0)
					{
						Date = DateTime.Parse(matches.GetByGroupName(nameof(Date).ToLower()).Replace("UTC", "GMT"));
						Description = matches.GetByGroupName(nameof(Description).ToLower());
						IsInDevelopment = isInDevelopment;
						Label = matches.GetByGroupName(nameof(Label).ToLower());
						Name = matches.GetByGroupName(Properties.Resources.RegEx_GroupName_CodeName);
						Origin = matches.GetByGroupName(nameof(Origin).ToLower());
						Suite = matches.GetByGroupName(nameof(Suite).ToLower());
						Version = matches.GetByGroupName(nameof(Version).ToLower());
						string[] architectures = matches.GetByGroupName(Properties.Resources.RegEx_GroupName_Architectures).Split(' ');
						string[] components = matches.GetByGroupName(Properties.Resources.RegEx_GroupName_Components).Split(' ');
						foreach (string architecture in matches.GetByGroupName(Properties.Resources.RegEx_GroupName_Architectures).Split(' '))
							Items.Add(new ArchitectureInfo(this, baseUrl, components, architecture));
					}
				}
			}
		}

		#endregion
	}
}
