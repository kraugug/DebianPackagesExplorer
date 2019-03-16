/*
 * Copyright(C) 2019, Michal Heczko All rights reserved.
 *
 * This software may be modified and distributed under the terms of the
 * GNU General Public License v3.0. See the LICENSE file for details.
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DebianPackagesExplorer
{
	public class StringCollection : ReadOnlyCollection<string>
	{
		#region Constants

		public const string DefaultDelimiter = ",";

		#endregion

		#region Methods

		public static StringCollection Parse(string str)
		{
			return Parse(str, DefaultDelimiter);
		}

		public static StringCollection Parse(string str, string delimiter)
		{
			return new StringCollection(str.Split(new string[] { delimiter }, StringSplitOptions.RemoveEmptyEntries));
		}

		public override string ToString()
		{
			return string.Join(string.Format("{0} ", DefaultDelimiter), this);
		}

		public string ToString(string delimiter)
		{
			return string.Join(string.Format("{0} ", delimiter), this);
		}

		#endregion

		#region Constructor

		private StringCollection(IList<string> source) : base(source.Select(i => i.Trim()).ToList())
		{ }

		#endregion

		#region Operators

		public static implicit operator StringCollection(string str)
		{
			return Parse(str, DefaultDelimiter);
		}

		//public static implicit operator string(StringCollection collection)
		//{
		//	return collection.ToString();
		//}

		#endregion
	}
}
