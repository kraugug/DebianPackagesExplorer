/*
 * Copyright(C) 2019, Michal Heczko All rights reserved.
 *
 * This software may be modified and distributed under the terms of the
 * GNU General Public License v3.0. See the LICENSE file for details.
 */

using System.Collections.ObjectModel;

namespace DebianPackagesExplorer.Debian
{
    public class ArchitectureInfo
    {
		#region Properties

		private string BaseUrl { get; set; }

		public ObservableCollection<ComponentInfo> Components { get; }

		public string Name { get; }

		public CodeNameInfo Parent { get; }

		#endregion

		#region Methods

		public override string ToString()
		{
			return Name;
		}

		#endregion

		#region Constructrors

		public ArchitectureInfo(CodeNameInfo parent, string baseUrl, string[] components, string name)
		{
			Components = new ObservableCollection<ComponentInfo>();
			BaseUrl = baseUrl;
			Name = name;
			Parent = parent;
			foreach (string component in components)
				Components.Add(new ComponentInfo(this, baseUrl, Name, component));
		}

		#endregion
	}
}
