using DebianPackagesExplorer.Extensions;
using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace DebianPackagesExplorer.ServerFileSystem
{
	public class ReleaseFile
	{
		#region Properties

		public string[] Architectures { get; }

		public string[] Components { get; }

		public DateTime Date { get; }

		public string Description { get; }

		public string FullName { get { return ToString(); } }

		public string Label { get; }

		public string Origin { get; }

		public string Suite { get; }

		public string Version { get; }

		#endregion

		#region Methods

		public static ReleaseFile Parse(string url)
		{
			if (!url.EndsWith("Release"))
				url = string.Format("{0}/Release", url);
			WebRequest webRequest = WebRequest.Create(url);
			{
				using (Stream stream = webRequest.GetResponse().GetResponseStream())
				using (StreamReader reader = new StreamReader(stream))
				{
					MatchCollection matches = Regex.Matches(reader.ReadToEnd(), Properties.Resources.RegEx_Pattern_ReleaseFile);
					if (matches.Count > 0)
						return new ReleaseFile(
							matches.GetByGroupName(Properties.Resources.RegEx_GroupName_Architectures).Split(' '),
							matches.GetByGroupName(Properties.Resources.RegEx_GroupName_Components).Split(' '),
							DateTime.Parse(matches.GetByGroupName(nameof(Date).ToLower()).Replace("UTC", "GMT")),
							matches.GetByGroupName(nameof(Description).ToLower()),
							matches.GetByGroupName(nameof(Label).ToLower()),
							matches.GetByGroupName(Properties.Resources.RegEx_GroupName_CodeName),
							matches.GetByGroupName(nameof(Origin).ToLower()),
							matches.GetByGroupName(nameof(Suite).ToLower()),
							matches.GetByGroupName(nameof(Version).ToLower()));
				}
			}
			return null;
		}

		#endregion

		#region Constructor

		public ReleaseFile(string[] architectures, string[] components, DateTime date, string description, string label, string name, string origin, string suite, string version)
		{
			Architectures = architectures;
			Components = components;
			Date = date;
			Description = description;
			Label = label;
			Origin = origin;
			Suite = suite;
			Version = version;
		}

		#endregion
	}
}
