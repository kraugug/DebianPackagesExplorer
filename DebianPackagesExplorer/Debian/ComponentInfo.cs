/*
 * Copyright(C) 2019, Michal Heczko All rights reserved.
 *
 * This software may be modified and distributed under the terms of the
 * GNU General Public License v3.0. See the LICENSE file for details.
 */

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
