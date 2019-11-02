/*
 * Copyright(C) 2019, Michal Heczko All rights reserved.
 *
 * This software may be modified and distributed under the terms of the
 * GNU General Public License v3.0. See the LICENSE file for details.
 */

namespace DebianPackagesExplorer.ServerFileSystem
{
    public class ComponentInfo : FolderInfo
	{
		#region Properties

		public ArchitectureInfo Parent { get; }

		public string Url { get; }

		#endregion

		#region Methods

		public override string ToString() => Name;

		#endregion

		#region Constructrors

		public ComponentInfo(ArchitectureInfo parent, string baseUrl, string architecture, string name) : base(name, baseUrl)
		{
			Parent = parent;
			Url = string.Format("{0}/{1}/binary-{2}/Packages.gz", baseUrl, name, architecture);
		}

		#endregion
	}
}
