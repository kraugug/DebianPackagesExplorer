using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DebianPackagesExplorer
{
	public class UseParseFunctionAttribute : Attribute
	{
		#region Property

		public string Delimiter { get; }

		#endregion

		#region Constructor

		public UseParseFunctionAttribute(string delimiter)
		{
			Delimiter = delimiter;
		}

		#endregion
	}
}
