/*
 * Copyright(C) 2019, Michal Heczko All rights reserved.
 *
 * This software may be modified and distributed under the terms of the
 * GNU General Public License v3.0. See the LICENSE file for details.
 */

using System;

namespace DebianPackagesExplorer.Tools
{
	public class HistoryPoolEventArgs<T> : EventArgs
	{
		#region Properties

		public HistoryPoolAction Action { get; }

		public T NewItem { get; }

		public T OldItem { get; }

		#endregion

		#region Contructor

		public HistoryPoolEventArgs(HistoryPoolAction action, T newItem, T oldItem)
		{
			Action = action;
			NewItem = newItem;
			OldItem = oldItem;
		}

		#endregion
	}
}
