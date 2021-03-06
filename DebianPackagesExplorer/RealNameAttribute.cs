﻿/*
 * Copyright(C) 2019, Michal Heczko All rights reserved.
 *
 * This software may be modified and distributed under the terms of the
 * GNU General Public License v3.0. See the LICENSE file for details.
 */

using System;

namespace DebianPackagesExplorer
{
	public class RealNameAttribute : Attribute
	{
		#region Properties

		public string Name { get; }

		#endregion

		#region Constructor

		public RealNameAttribute(string name)
		{
			Name = name;
		}

		#endregion
	}
}
