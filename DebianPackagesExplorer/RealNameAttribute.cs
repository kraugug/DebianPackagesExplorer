using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
