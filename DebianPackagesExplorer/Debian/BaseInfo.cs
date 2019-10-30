/*
 * Copyright(C) 2019, Michal Heczko All rights reserved.
 *
 * This software may be modified and distributed under the terms of the
 * GNU General Public License v3.0. See the LICENSE file for details.
 */

namespace DebianPackagesExplorer.Debian
{
	public abstract class BaseInfo
	{
		#region Properties

		public virtual string Name { get; protected set; }

		#endregion

		#region Constructor

		public BaseInfo(string name) => Name = name;

		#endregion
	}
}
