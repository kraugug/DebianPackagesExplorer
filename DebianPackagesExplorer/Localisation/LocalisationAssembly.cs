/*
 * Copyright(C) 2018, Michal Heczko All rights reserved.
 *
 * This software may be modified and distributed under the terms of the
 * GNU General Public License v3.0. See the LICENSE file for details.
 */

using System;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace DebianPackagesExplorer.Localisation
{
	public class LocalisationAssembly
	{
		#region Fields

		private AssemblyName m_AssemblyName;
		private string m_Name;
		private string m_ResourceSourceString;

		#endregion

		#region Properties

		public string Author { get; }

		public string FileName { get; }

		public string FileNameWithoutExtension { get { return System.IO.Path.GetFileNameWithoutExtension(FileName); } }

		public string FileNameWithoutPath { get { return System.IO.Path.GetFileName(FileName); } }

		public string Name { get { return string.IsNullOrEmpty(m_Name) ? m_AssemblyName.Name : m_Name; } }

		public Uri ResourceSource { get; }

		public string Version { get { return m_AssemblyName.Version.ToString(); } }

		#endregion

		#region Methods

		public void Apply()
		{
			Assembly assembly = Assembly.LoadFile(FileName);
			if (assembly != null)
			{
				var results = Application.Current.Resources.MergedDictionaries.Where(i => (i.Source != null) && !string.IsNullOrEmpty(i.Source.OriginalString) && i.Source.OriginalString.ToUpper().EndsWith(Properties.Resources.String_MainLocalisationFile));
				if (results.Count() > 0)
				{
					ResourceDictionary rd = results.First();
					Application.Current.Resources.MergedDictionaries.Remove(rd);
				}
				Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = ResourceSource });
			}
		}

		#endregion

		#region Constructor(s)

		public LocalisationAssembly(string fileName)
		{
			// TODO: Check resources for localization...
			m_AssemblyName = AssemblyName.GetAssemblyName(fileName);
			FileName = fileName;
			string name = m_AssemblyName.Name;
			if (string.IsNullOrEmpty(name))
			{
				name = System.IO.Path.GetFileName(FileName);
				int index = name.LastIndexOf('.');
				if (index > 0)
				{
					name = name.Substring(0, index);
				}
			}
			m_ResourceSourceString = string.Format(Properties.Resources.PackString_Localisation, name);
			ResourceSource = new Uri(m_ResourceSourceString, UriKind.RelativeOrAbsolute);
		}

		public LocalisationAssembly(string name, string fileName) : this(fileName)
		{
			m_Name = name;
		}

		#endregion
	}
}
