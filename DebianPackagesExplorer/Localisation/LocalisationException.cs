/*
 * Copyright(C) 2018, Michal Heczko All rights reserved.
 *
 * This software may be modified and distributed under the terms of the
 * GNU General Public License v3.0. See the LICENSE file for details.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebianPackagesExplorer.Localisation
{
	public class LocalisationException : Exception
	{
		#region Constructor

		public LocalisationException(string message) : base(message)
		{ }

		#endregion
	}
}
