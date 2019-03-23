using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebianPackagesExplorer.Debian
{
	public class SelectedSourcePackageInfo
	{
		#region Properties

		public string Architecture { get; }

		public string BaseUrl { get { return Url.Substring(0, Url.IndexOf("dists")); } }

		public string CodeName { get; }

		public string Component { get; }

		public string Url { get; }

		#endregion

		#region Methods

		#endregion

		#region Constructors

		public SelectedSourcePackageInfo(ComponentInfo componentInfo)
		{
			Architecture = componentInfo.Parent.Name;
			CodeName = componentInfo.Parent.Parent.Name;
			Component = componentInfo.Name;
			Url = componentInfo.Url;
		}

		#endregion
	}
}
