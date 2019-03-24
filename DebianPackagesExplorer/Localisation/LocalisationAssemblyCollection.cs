/*
 * Copyright(C) 2018, Michal Heczko All rights reserved.
 *
 * This software may be modified and distributed under the terms of the
 * GNU General Public License v3.0. See the LICENSE file for details.
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DebianPackagesExplorer.Localisation
{
	public class LocalisationAssemblyCollection : ObservableCollection<LocalisationAssembly>
	{
		#region Properties

		public static LocalisationAssemblyCollection Instance { get; private set; }

		#endregion

		#region Methods

		public void AddDefault(string defaultLocalizationName)
		{
			Add(new LocalisationAssembly(defaultLocalizationName, Assembly.GetEntryAssembly().Location));
			Apply(defaultLocalizationName);
		}

		public void Apply(string localizationName)
		{
			this[localizationName]?.Apply();
		}

		public void Apply(LocalisationAssembly localization)
		{
			this[localization.Name]?.Apply();
		}

		#endregion

		#region Constructor(s)

		public LocalisationAssemblyCollection() : this(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Properties.Resources.String_Localisations))
		{ }

		public LocalisationAssemblyCollection(string localisationsFolder)
		{
			if (Instance != null)
				throw new LocalisationException(Properties.Resources.Exception_LocalisationAlreadyCreated);
			Instance = this;
			// Load available localizations...
			if (Directory.Exists(localisationsFolder))
			{
				IEnumerable<string> files = Directory.GetFiles(localisationsFolder).Where(f => f.ToUpper().EndsWith(".DLL"));
				foreach (var file in files)
				{
					try
					{
						Add(new LocalisationAssembly(file));
					}
					catch (Exception ex)
					{
						Debug.WriteLine("{0}:\n\t{1}: {2}", new object[] { nameof(LocalisationAssemblyCollection), ex.GetType().FullName, ex.Message });
					}
				}
			}
		}

		#endregion

		#region Indexers

		public LocalisationAssembly this[string localizationName]
		{
			get { return this.Where(i => i.Name.CompareTo(localizationName) == 0).FirstOrDefault(); }
		}

		#endregion

		#region Nested types

		public class SimpleAssemblyLoader : MarshalByRefObject
		{
			public void Load(string path)
			{
				ValidatePath(path);
				Assembly.Load(path);
			}

			public void LoadFrom(string path)
			{
				ValidatePath(path);
				Assembly.LoadFrom(path);
			}

			private void ValidatePath(string path)
			{
				if (path == null)
					throw new ArgumentNullException(nameof(path));
				if (!File.Exists(path))
					throw new ArgumentException(string.Format("\"{0}\" does not exist", path), nameof(path));
			}
		}

		#endregion
	}
}
