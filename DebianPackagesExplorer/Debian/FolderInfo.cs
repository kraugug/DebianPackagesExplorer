/*
 * Copyright(C) 2019, Michal Heczko All rights reserved.
 *
 * This software may be modified and distributed under the terms of the
 * GNU General Public License v3.0. See the LICENSE file for details.
 */

using System.Collections.ObjectModel;

namespace DebianPackagesExplorer.Debian
{
	public class FolderInfo : BaseInfo
	{
		#region Properties

		public virtual string BaseUrl { get; private set; }

		public ObservableCollection<BaseInfo> Items { get; }

		#endregion

		#region Constructor

		public FolderInfo(string name, string baseUrl) : base(name)
		{
			BaseUrl = baseUrl;
			Items = new ObservableCollection<BaseInfo>();
		}

		#endregion
	}
}
