﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DebianPackagesExplorer.Windows
{
	/// <summary>
	/// Interaction logic for OpenLinkWindow.xaml
	/// </summary>
	public partial class OpenLinkWindow : Window
	{
		#region Fileds

		public static readonly RoutedCommand CommandOk = new RoutedCommand();

		#endregion

		#region Properties

		public string Link { get; set; }

		#endregion

		#region Methods

		private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = !string.IsNullOrEmpty(Link);
		}

		private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Uri testUri;
			if (Uri.TryCreate(Link, UriKind.Absolute, out testUri))
			{
				if (!Properties.Settings.Default.LinkHistory.Contains(Link))
					Properties.Settings.Default.LinkHistory.Add(Link);
				DialogResult = true;
			}
			else
				MessageBox.Show("Invalid link!", Title, MessageBoxButton.OK, MessageBoxImage.Error);
		}

		#endregion

		#region Constructor

		public OpenLinkWindow(Window owner)
		{
			DataContext = this;
			Owner = owner;
			InitializeComponent();
		}

		#endregion
	}
}
