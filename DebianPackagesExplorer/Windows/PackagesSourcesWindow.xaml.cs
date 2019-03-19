using System;
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
	/// Interaction logic for PackagesSourcesWindow.xaml
	/// </summary>
	public partial class PackagesSourcesWindow : Window
	{
		#region Fields

		public static readonly RoutedCommand CommandAdd = new RoutedCommand();
		public static readonly RoutedCommand CommandOk = new RoutedCommand();
		public static readonly RoutedCommand CommandRefresh = new RoutedCommand();

		#endregion

		#region Properties

		public string PackageFilename { get; }

		#endregion

		#region Methods

		private void CommandAdd_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = !string.IsNullOrEmpty(ComboBoxSources.Text) && !Properties.Settings.Default.Sources.Contains(ComboBoxSources.Text);
		}

		private void CommandAdd_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Properties.Settings.Default.Sources.Add(ComboBoxSources.Text);
		}

		private void CommandOk_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			DialogResult = true;
		}

		private void CommandRefresh_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = !string.IsNullOrEmpty(ComboBoxSources.Text);
		}

		private void CommandRefresh_Executed(object sender, ExecutedRoutedEventArgs e)
		{

		}

		#endregion

		#region Constructor

		public PackagesSourcesWindow()
		{
			DataContext = this;
			Owner = App.MainWindow;
			InitializeComponent();
		}

		#endregion
	}
}
