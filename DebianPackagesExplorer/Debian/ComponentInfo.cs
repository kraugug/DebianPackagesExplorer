using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DebianPackagesExplorer.Debian
{
    public class ComponentInfo
    {
		#region Properties

		public string BaseUrl { get; }

		public string Name { get; }

		public ArchitectureInfo Parent { get; }

		public string Url { get; }

		#endregion

		#region Methods

		public override string ToString()
		{
			return Name;
		}

		#endregion

		#region Constructrors

		public ComponentInfo(ArchitectureInfo parent, string baseUrl, string architecture, string name)
		{
			BaseUrl = baseUrl;
			Name = name;
			Parent = parent;
			Url = string.Format("{0}/{1}/binary-{2}/Packages.gz", baseUrl, name, architecture);
		}

		#endregion
	}
}
