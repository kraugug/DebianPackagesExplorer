using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DebianPackagesExplorer.Extensions
{
	public static class PropertyInfoExtensions
	{
		#region Methods

		public static TAttribute GetCustomAttribute<TAttribute>(this PropertyInfo property)
		{
			return (TAttribute)property.GetCustomAttributes(false).Where(a => a is TAttribute).FirstOrDefault();
		}

		#endregion
	}
}
