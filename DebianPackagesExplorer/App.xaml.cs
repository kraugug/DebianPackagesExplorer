/*
 * Copyright(C) 2019, Michal Heczko All rights reserved.
 *
 * This software may be modified and distributed under the terms of the
 * GNU General Public License v3.0. See the LICENSE file for details.
 */

using DebianPackagesExplorer.Debian;
using DebianPackagesExplorer.Windows;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace DebianPackagesExplorer
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		#region Properties

		public static new App Current { get { return (App)Application.Current; } }

		public static new MainWindow MainWindow { get { return (MainWindow)Application.Current.MainWindow; } }

		#endregion

		#region Methods

		private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{
			MessageBox.Show(MainWindow, e.Exception.Message, e.Exception.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
			e.Handled = true;
		}

		public TType GetResource<TType>(string resourceKey)
		{
			return (TType)Resources[resourceKey];
		}

		protected override void OnExit(ExitEventArgs e)
		{
			DebianPackagesExplorer.Properties.Settings.Default.Save();
			base.OnExit(e);
		}

		#endregion

		#region Constructor

		public App()
		{
			if (DebianPackagesExplorer.Properties.Settings.Default.LinkHistory == null)
				DebianPackagesExplorer.Properties.Settings.Default.LinkHistory = new System.Collections.Specialized.StringCollection();
			if (DebianPackagesExplorer.Properties.Settings.Default.Sources == null)
				DebianPackagesExplorer.Properties.Settings.Default.Sources = new System.Collections.Specialized.StringCollection();
			Localisation.LocalisationAssemblyCollection loc = new Localisation.LocalisationAssemblyCollection();
			//loc.AddDefault("English");
			//loc.Apply("English");
			loc.Apply("Czech");
		}

		#endregion

		#region Indexers

		public string this[string resourceKey] { get { return Resources[resourceKey] as string; } }

		#endregion
	}
}
