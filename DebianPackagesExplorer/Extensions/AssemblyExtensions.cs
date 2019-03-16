/*
 * Copyright(C) 2018, Michal Heczko All rights reserved.
 *
 * This software may be modified and distributed under the terms of the
 * GNU General Public License v3.0. See the LICENSE file for details.
 */

using System.Reflection;

namespace DebianPackagesExplorer.Extensions
{
	public static class AssemblyExtensions
	{
		#region Methods

		public static string GetAttributeValue(this Assembly assembly, AssemblyAttribute attribute)
		{
			if (assembly == null)
				return null;
			switch (attribute)
			{
				case AssemblyAttribute.Company:
					return ((AssemblyCompanyAttribute)assembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false)[0]).Company;
				case AssemblyAttribute.Configuration:
					return ((AssemblyConfigurationAttribute)assembly.GetCustomAttributes(typeof(AssemblyConfigurationAttribute), false)[0]).Configuration;
				case AssemblyAttribute.Copyright:
					return ((AssemblyCopyrightAttribute)assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0]).Copyright;
				case AssemblyAttribute.Culture:
					return ((AssemblyCultureAttribute)assembly.GetCustomAttributes(typeof(AssemblyCultureAttribute), false)[0]).Culture;
				case AssemblyAttribute.Description:
					return ((AssemblyDescriptionAttribute)assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)[0]).Description;
				case AssemblyAttribute.Product:
					return ((AssemblyProductAttribute)assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false)[0]).Product;
				case AssemblyAttribute.Title:
					return ((AssemblyTitleAttribute)assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0]).Title;
				case AssemblyAttribute.Trademark:
					return ((AssemblyTrademarkAttribute)assembly.GetCustomAttributes(typeof(AssemblyTrademarkAttribute), false)[0]).Trademark;
				default:
					return null;
			}
		}

		#endregion

		#region Nested types

		public enum AssemblyAttribute
		{
			Company,
			Configuration,
			Copyright,
			Culture,
			Description,
			Product,
			Title,
			Trademark,
		}

		#endregion
	}
}
