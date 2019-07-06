/*
 * Copyright(C) 2019, Michal Heczko All rights reserved.
 *
 * This software may be modified and distributed under the terms of the
 * GNU General Public License v3.0. See the LICENSE file for details.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebianPackagesExplorer.Tools
{
    public class HistoryPool<T>
    {
		#region Properties

		public bool CanGoNext { get { return (Position < Pool.Count) && (Position != Pool.Count - 1); } }

		public bool CanGoPrevious { get { return Position > 0; } }

		public T Current { get { return Pool.Count > 0 ? Pool[Position] : default(T); } }

		public T First { get { return Pool.Count > 0 ? Pool[0] : default(T); } }

		public bool IsBrowsing { get; private set; }

		public T Last { get { return Pool.Count > 0 ? Pool[Pool.Count - 1] : default(T); } }

		protected List<T> Pool { get; }

		public int PoolLimit
		{
			get { return m_PoolLimit; }
			set
			{
				if (value > 1)
				{
					m_PoolLimit = value;
					if (Pool.Count > m_PoolLimit)
						Pool.RemoveRange(0, Pool.Count - m_PoolLimit);
				}
				else
					throw new ArgumentOutOfRangeException(nameof(PoolLimit));
			}
		}
		private int m_PoolLimit;

		/// <summary>
		/// Zero based.
		/// </summary>
		protected int Position
		{
			get { return m_Position; }
			set
			{
				if ((value < Pool.Count) && (value >= 0))
					m_Position = value;
				else
					throw new IndexOutOfRangeException();
			}
		}
		private int m_Position;

		#endregion

		#region Methods

		public void Add(T item)
		{
			Pool.Add(item);
			Position = Pool.Count - 1;
		}

		public void Clear() => Pool.Clear();

		protected void DoGo(HistoryPoolAction action, T newItem, T oldItem) => Go?.Invoke(this, new HistoryPoolEventArgs<T>(action, newItem, oldItem));

		public void Next()
		{
			if (CanGoNext)
			{
				T temp = Current;
				Position += 1;
				IsBrowsing = true;
				DoGo(HistoryPoolAction.GoNext, Current, temp);
				IsBrowsing = false;
			}
		}

		public void Previous()
		{
			if (CanGoPrevious)
			{
				T temp = Current;
				Position -= 1;
				IsBrowsing = true;
				DoGo(HistoryPoolAction.GoPrevious, Current, temp);
				IsBrowsing = false;
			}
		}

		#endregion

		#region Constructor

		public HistoryPool()
		{
			Pool = new List<T>();
			m_Position = 0;
		}

		#endregion

		#region Events

		public event EventHandler<HistoryPoolEventArgs<T>> Go;

		#endregion
	}
}
