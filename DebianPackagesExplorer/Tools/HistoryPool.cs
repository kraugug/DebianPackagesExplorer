/*
 * Copyright(C) 2019, Michal Heczko All rights reserved.
 *
 * This software may be modified and distributed under the terms of the
 * GNU General Public License v3.0. See the LICENSE file for details.
 */

using DebianPackagesExplorer.Extensions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DebianPackagesExplorer.Tools
{
    public class HistoryPool<T> : INotifyPropertyChanged
    {
		#region Properties

		public bool CanGoNext { get { return (Position < Pool.Count) && (Position != Pool.Count - 1); } }

		public bool CanGoPrevious { get { return Position > 0; } }

		public T Current
		{
			get { return Pool.Count > 0 ? Pool[Position] : default(T); }
			set
			{
				int index = Pool.IndexOf(value);
				if (index != -1)
				{
					int oldPosition = Position;
					Position = index;
					IsBrowsing = true;
					FireGoEvent(HistoryPoolAction.CurrentChanged, Pool[index], Pool[oldPosition]);
					IsBrowsing = false;
				}
			}
		}

		public T First { get { return Pool.Count > 0 ? Pool[0] : default(T); } }

		public bool IsBrowsing { get; private set; }

		public T Last { get { return Pool.Count > 0 ? Pool[Pool.Count - 1] : default(T); } }

		public ObservableCollection<T> Pool { get; }

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
			FirePropertyChangedEvent(nameof(Current));
		}

		public void Clear()
		{
			Pool.Clear();
			FirePropertyChangedEvent(nameof(Current));
		}

		protected void FireGoEvent(HistoryPoolAction action, T newItem, T oldItem)
		{
			Go?.Invoke(this, new HistoryPoolEventArgs<T>(action, newItem, oldItem));
			FirePropertyChangedEvent(nameof(Current));
		}

		private void FirePropertyChangedEvent(params string[] propertyNames)
		{
			foreach (string propertyName in propertyNames)
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public void Next()
		{
			if (CanGoNext)
			{
				T temp = Current;
				Position += 1;
				IsBrowsing = true;
				FireGoEvent(HistoryPoolAction.GoNext, Current, temp);
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
				FireGoEvent(HistoryPoolAction.GoPrevious, Current, temp);
				IsBrowsing = false;
			}
		}

		#endregion

		#region Constructor

		public HistoryPool()
		{
			Pool = new ObservableCollection<T>();
			m_Position = 0;
		}

		#endregion

		#region Events

		public event EventHandler<HistoryPoolEventArgs<T>> Go;
		public event PropertyChangedEventHandler PropertyChanged;

		#endregion
	}
}
