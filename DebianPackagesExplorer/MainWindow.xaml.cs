using DebianPackagesExplorer.Debian;
using DebianPackagesExplorer.Extensions;
using DebianPackagesExplorer.Windows;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DebianPackagesExplorer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		#region Fields

		public static readonly RoutedCommand CommandFileBrowseDebianSources = new RoutedCommand();
		public static readonly RoutedCommand CommandFileExit = new RoutedCommand();
		public static readonly RoutedCommand CommandFileOpenFile = new RoutedCommand();
		public static readonly RoutedCommand CommandFileOpenLink = new RoutedCommand();
		public static readonly RoutedCommand CommandHelpAbout = new RoutedCommand();
		public static readonly RoutedCommand CommandToolsOptions = new RoutedCommand();

		#endregion

		#region Properties

		public PackagesCollection Packages { get; private set; }

		#endregion

		#region Methods

		#region Commands

		private void CommandFileExit_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Close();
		}

		private void CommandFileBrowseDebianSources_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			PackagesSourcesWindow dialog = new PackagesSourcesWindow();
			if (dialog.ShowDialog().Value)
			{

			}
		}

		private void CommandFileOpenFile_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.CheckFileExists = true;
			dialog.Filter = "Archives|*.gz|Text files|*.txt";
			if (dialog.ShowDialog().Value)
				ParseList(PackagesCollection.ParseArchive(dialog.FileName));
		}

		private void CommandFileOpenLink_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			OpenLinkWindow dialog = new OpenLinkWindow();
			if (dialog.ShowDialog().Value)
			{
				string tempFile = System.IO.Path.GetTempFileName();
				using (WebClient webClient = new WebClient())
				{
					ProgressBarStatus.Value = 0;
					TextBlockStatus.Text = string.Format("Downloading file {0}", dialog.Link);
					webClient.DownloadFileCompleted += (object sender1, AsyncCompletedEventArgs e1) =>
					{
						ProgressBarStatus.Value = 0;
						TextBlockStatus.Text = "Parsing file ...";
						if (e1.Error == null)
						{
							if (!e1.Cancelled)
								ParseList(PackagesCollection.ParseArchive(tempFile));
							TextBlockStatus.Text = "Ready";
							if (!Properties.Settings.Default.LinkHistory.Contains(dialog.Link))
								Properties.Settings.Default.LinkHistory.Add(dialog.Link);
						}
						else
							MessageBox.Show(this, TextBlockStatus.Text =  e1.Error.Message, e1.Error.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
					};
					webClient.DownloadProgressChanged += (object sender1, DownloadProgressChangedEventArgs e1) =>
					{
						ProgressBarStatus.Value = e1.ProgressPercentage;
					};
					webClient.DownloadFileAsync(new Uri(dialog.Link), tempFile);
				}
			}
		}

		private void CommandHelpAbout_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			(new Windows.AboutWindow()).ShowDialog();
		}

		private void CommandToolsOptions_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			OptionsWindow dialog = new OptionsWindow();
			if (dialog.ShowDialog().Value)
			{

			}
		}

		#endregion

		private void DataGridPackages_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			DataGrid dataGrid = sender as DataGrid;
			if (dataGrid != null)
			{
				string link = (dataGrid.SelectedItem as PackageInfo)?.Homepage;
				if (!string.IsNullOrEmpty(link))
					Process.Start(link);
			}
				
		}

		private void ParseList(IEnumerable<string> list)
		{
			Task.Factory.StartNew(() =>
			{
				Dispatcher.BeginInvoke(new Action(() =>
				{
					Packages.ParseListParallel(list);
				}));
			});
		}

		private void TextBoxHomepage_MouseDown(object sender, MouseButtonEventArgs e)
		{
			string link = (sender as TextBox).Text;
			if (!string.IsNullOrEmpty(link))
				Process.Start(link);
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{

		}

		#endregion

		#region Constructor

		public MainWindow()
		{
			DataContext = this;
			Packages = new PackagesCollection();
			InitializeComponent();
			Title = Assembly.GetExecutingAssembly().GetAttributeValue(AssemblyExtensions.AssemblyAttribute.Description);
		}

		#endregion
	}
}
