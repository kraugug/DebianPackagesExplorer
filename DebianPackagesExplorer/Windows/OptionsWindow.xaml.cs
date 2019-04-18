/*
 * Copyright(C) 2019, Michal Heczko All rights reserved.
 *
 * This software may be modified and distributed under the terms of the
 * GNU General Public License v3.0. See the LICENSE file for details.
 */

using DebianPackagesExplorer.Localisation;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DebianPackagesExplorer.Windows
{
    /// <summary>
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window
    {
		#region Fileds

		public static readonly RoutedCommand CommandBrowseForFolder = new RoutedCommand();
		public static readonly RoutedCommand CommandOk = new RoutedCommand();
		public static readonly RoutedCommand CommandOpenFolder = new RoutedCommand();

		#endregion

		#region Methods

		private void CommandBrowseForFolder_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
			if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				Properties.Settings.Default.DefaultDownloadsFolder = dialog.SelectedPath;
		}

		private void CommandOk_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			DialogResult = true;
		}

		private void CommandOpenFolder_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Process.Start(Properties.Settings.Default.DefaultDownloadsFolder);
		}

		private void ListViewLanguages_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			Properties.Settings.Default.Localisation = ((sender as ListView).SelectedItem as LocalisationAssembly).Name;
			App.Localisations.Apply((sender as ListView).SelectedItem as LocalisationAssembly);
		}

		#endregion

		#region Constructor

		public OptionsWindow()
        {
			DataContext = this;
			Owner = App.MainWindow;
			InitializeComponent();
        }

		#endregion
	}
}
