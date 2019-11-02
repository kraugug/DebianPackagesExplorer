/*
 * Copyright(C) 2019, Michal Heczko All rights reserved.
 *
 * This software may be modified and distributed under the terms of the
 * GNU General Public License v3.0. See the LICENSE file for details.
 */

using System;
using System.Collections.ObjectModel;

namespace DebianPackagesExplorer.Extensions
{
	public static class ObservableCollectionExtensions
	{
		#region Methods

		public static void RemoveRange<T>(this ObservableCollection<T> collection, int index, int count)
		{
			if (collection == null)
				throw new ArgumentNullException(nameof(collection));
			for (int i = 0; i < count; i++)
				collection.RemoveAt(index);
		}

		#endregion
	}
}
