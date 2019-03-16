using DebianPackagesExplorer.Debian;
using DebianPackagesExplorer.Extensions;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
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

		public static readonly RoutedCommand CommandFileExit = new RoutedCommand();
		public static readonly RoutedCommand CommandFileOpenFile = new RoutedCommand();
		public static readonly RoutedCommand CommandFileOpenLink = new RoutedCommand();
		public static readonly RoutedCommand CommandHelpAbout = new RoutedCommand();

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

		private void CommandFileOpenFile_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.CheckFileExists = true;
			dialog.Filter = "Archives|*.gz|Text files|*.txt";
			if (dialog.ShowDialog().Value)
			{
				IEnumerable<string> packagesInfo = PackagesCollection.ParseArchive(dialog.FileName);
				Packages.ParseListParallel(packagesInfo);
			}
		}

		private void CommandFileOpenLink_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			BorderOpenFileLink.Visibility = Visibility.Visible;
		}

		private void CommandHelpAbout_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			(new Windows.AboutWindow()).ShowDialog();
		}

		#endregion

		private void TextBoxOpenFileLink_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				string tempFile = System.IO.Path.GetTempFileName();
				using (WebClient webClient = new WebClient())
				{
					ProgressBarStatus.Value = 0;
					TextBlockStatus.Text = string.Format("Downloading file {0}", TextBoxOpenFileLink.Text);
					webClient.DownloadFileCompleted += (object sender1, AsyncCompletedEventArgs e1) =>
					{
						ProgressBarStatus.Value = 0;
						TextBlockStatus.Text = "Parsing file ...";
						if (!e1.Cancelled)
							Packages.ParseListParallel(PackagesCollection.ParseArchive(tempFile));
						TextBlockStatus.Text = "Ready";
						BorderOpenFileLink.Visibility = Visibility.Collapsed;
					};
					webClient.DownloadProgressChanged += (object sender1, DownloadProgressChangedEventArgs e1) =>
					{
						ProgressBarStatus.Value = e1.ProgressPercentage;
					};
					webClient.DownloadFileAsync(new Uri(TextBoxOpenFileLink.Text), tempFile);
				}
			}
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
