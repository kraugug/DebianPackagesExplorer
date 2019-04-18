using DebianPackagesExplorer.Debian;
using DebianPackagesExplorer.Extensions;
using DebianPackagesExplorer.Windows;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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

using FolderBrowserDialog = System.Windows.Forms.FolderBrowserDialog;

namespace DebianPackagesExplorer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		#region Fields

		public static readonly RoutedCommand CommandDownloadPackage = new RoutedCommand();
		public static readonly RoutedCommand CommandFileBrowseDebianSources = new RoutedCommand();
		public static readonly RoutedCommand CommandFileExit = new RoutedCommand();
		public static readonly RoutedCommand CommandFileOpenFile = new RoutedCommand();
		public static readonly RoutedCommand CommandFileOpenLink = new RoutedCommand();
		public static readonly RoutedCommand CommandHelpAbout = new RoutedCommand();
		public static readonly RoutedCommand CommandOpenContainingFolder = new RoutedCommand();
		public static readonly RoutedCommand CommandToolsOptions = new RoutedCommand();

		#endregion

		#region Properties

		public bool IsDownloading { get; set; }

		public PackagesCollection Packages { get; private set; }

		public PackageInfo SelectedPackage { get { return DataGridPackages.SelectedItem as PackageInfo; } }

		#endregion

		#region Methods

		private void ComboBoxPackageInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ComboBox comboBox = sender as ComboBox;
			if (comboBox != null && comboBox.SelectedItem != null)
			{
				Match match = Regex.Match(comboBox.SelectedItem as string, @"(?<name>.*)\s+\((?<mode>[><=]+)\s+(?<version>[0-9.]+)\)");
				if (match != null && match.Success)
				{
					PackageInfo package = Packages[match.Groups["name"].Value];
					if (package != null)
					{
						Debug.WriteLine(string.Format("Package: {0}\nVersion: {1}\nComparison: {2}", match.Groups["name"].Value, match.Groups["version"].Value, match.Groups["mode"].Value));
						DataGridPackages.SelectedItem = package;
						DataGridPackages.ScrollIntoView(package);
					}
				}
			}
		}

		#region Commands

		private void CommandDownloadPackage_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = !string.IsNullOrEmpty(Packages.GetPackageDownloadLink(SelectedPackage)) && !IsDownloading;
		}

		private void CommandDownloadPackage_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if (Properties.Settings.Default.UseDefaultDownloadsFolder)
			{
				DownloadPackage(Packages.GetPackageDownloadLink(SelectedPackage),
					System.IO.Path.Combine(Properties.Settings.Default.DefaultDownloadsFolder,
					System.IO.Path.Combine(Packages.SourceInfo.CodeName, Packages.SourceInfo.Component, Packages.SourceInfo.Architecture, SelectedPackage.FileName)));
			}
			else
			{
				FolderBrowserDialog dialog = new FolderBrowserDialog();
				if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
					DownloadPackage(Packages.GetPackageDownloadLink(SelectedPackage), System.IO.Path.Combine(dialog.SelectedPath, SelectedPackage.FileName));
			}
		}

		private void CommandFileExit_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Close();
		}

		private void CommandFileBrowseDebianSources_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			PackagesSourcesWindow dialog = new PackagesSourcesWindow();
			if (dialog.ShowDialog().Value)
			{
				Packages.Clear();
				OpenRemotePackagesFile(dialog.SelectedPackagesSource);
			}
		}

		private void CommandFileOpenFile_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.CheckFileExists = true;
			dialog.Filter = App.GetResource<string>(Properties.Resources.ResKey_String_OpenFileDialogFilter);
			if (dialog.ShowDialog().Value)
			{
				Packages.SourceInfo = null;
				ParseList(PackagesCollection.ParseArchive(dialog.FileName));
			}
		}

		private void CommandFileOpenLink_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			OpenLinkWindow dialog = new OpenLinkWindow();
			if (dialog.ShowDialog().Value)
			{
				Packages.SourceInfo = SelectedSourcePackageInfo.Parse(dialog.Link);
				OpenRemotePackagesFile(dialog.Link);
			}
		}

		private void CommandHelpAbout_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			(new Windows.AboutWindow()).ShowDialog();
		}

		private void CommandOpenContainingFolder_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			//e.CanExecute = false;
			if (Properties.Settings.Default.UseDefaultDownloadsFolder)
			{
				PackageInfo package = DataGridPackages.SelectedItem as PackageInfo;
				e.CanExecute = ((package != null) && (Packages.SourceInfo != null)) && File.Exists(System.IO.Path.Combine(Properties.Settings.Default.DefaultDownloadsFolder,
					System.IO.Path.Combine(Packages.SourceInfo.CodeName, Packages.SourceInfo.Component, Packages.SourceInfo.Architecture, SelectedPackage.FileName)));
			}
		}

		private void CommandOpenContainingFolder_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Process.Start("explorer.exe", "/select," + System.IO.Path.Combine(Properties.Settings.Default.DefaultDownloadsFolder,
				System.IO.Path.Combine(Packages.SourceInfo.CodeName, Packages.SourceInfo.Component, Packages.SourceInfo.Architecture, SelectedPackage.FileName)));
		}

		private void CommandToolsOptions_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			(new OptionsWindow()).ShowDialog();
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

		public void DownloadPackage(string url, string fileName)
		{
			bool go = true;
			if (Properties.Settings.Default.ConfirmFileOverwrite && File.Exists(fileName))
				go = MessageBox.Show("Do wish to overwrite existing file?", "Download Package", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
			if (go)
				using (WebClient webClient = new WebClient())
				{
					ProgressBarStatus.Value = 0;
					ProgressBarStatus.IsIndeterminate = true;
					TextBlockStatus.Text = string.Format(App.GetResource<string>(Properties.Resources.ResKey_String_DownloadingFile_Formatted), url);
					IsDownloading = true;
					webClient.DownloadFileCompleted += (object sender1, AsyncCompletedEventArgs e1) =>
					{
						ProgressBarStatus.Value = 0;
						ProgressBarStatus.IsIndeterminate = false;
						if (e1.Error == null)
						{
							TextBlockStatus.Text = App.GetResource<string>(Properties.Resources.ResKey_String_DownloadCompleted);
							IsDownloading = false;
							CommandManager.InvalidateRequerySuggested();
						}
						else
							MessageBox.Show(this, TextBlockStatus.Text = e1.Error.Message, e1.Error.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
					};
					webClient.DownloadProgressChanged += (object sender1, DownloadProgressChangedEventArgs e1) =>
					{
						if ((e1.ProgressPercentage > 0) && ProgressBarStatus.IsIndeterminate)
							ProgressBarStatus.IsIndeterminate = false;
						ProgressBarStatus.Value = e1.ProgressPercentage;
					};
					if (!Directory.Exists(System.IO.Path.GetDirectoryName(fileName)))
						Directory.CreateDirectory(System.IO.Path.GetDirectoryName(fileName));
					webClient.DownloadFileAsync(new Uri(url), fileName);
				}
		}

		public void OpenRemotePackagesFile(SelectedSourcePackageInfo sourceInfo)
		{
			Packages.SourceInfo = sourceInfo;
			OpenRemotePackagesFile(sourceInfo.Url);
		}

		public void OpenRemotePackagesFile(string url)
		{
			string tempFile = System.IO.Path.GetTempFileName();
			using (WebClient webClient = new WebClient())
			{
				ProgressBarStatus.Value = 0;
				ProgressBarStatus.IsIndeterminate = true;
				TextBlockStatus.Text = string.Format(App.GetResource<string>(Properties.Resources.ResKey_String_DownloadingFile_Formatted), url);
				webClient.DownloadFileCompleted += (object sender1, AsyncCompletedEventArgs e1) =>
				{
					ProgressBarStatus.Value = 0;
					ProgressBarStatus.IsIndeterminate = false;
					TextBlockStatus.Text = App.GetResource<string>(Properties.Resources.ResKey_String_ParsingFile);
					if (e1.Error == null)
					{
						if (!e1.Cancelled)
						{
							ParseList(PackagesCollection.ParseArchive(tempFile));
							if (!Properties.Settings.Default.LinkHistory.Contains(url))
								Properties.Settings.Default.LinkHistory.Add(url);
						}
					}
					else
					{
						ProgressBarStatus.IsIndeterminate = false;
						MessageBox.Show(this, TextBlockStatus.Text = e1.Error.Message, e1.Error.GetType().Name, MessageBoxButton.OK, MessageBoxImage.Error);
					}
				};
				webClient.DownloadProgressChanged += (object sender1, DownloadProgressChangedEventArgs e1) =>
				{
					if ((e1.ProgressPercentage > 0) && ProgressBarStatus.IsIndeterminate)
						ProgressBarStatus.IsIndeterminate = false;
					ProgressBarStatus.Value = e1.ProgressPercentage;
				};
				webClient.DownloadFileAsync(new Uri(url), tempFile);
			}
		}

		private void ParseList(IEnumerable<string> source)
		{
			using (BackgroundWorker parser = new BackgroundWorker())
			{
				int progress = 1;
				int progressMax = source.Count();
				parser.DoWork += (object sender, DoWorkEventArgs e) =>
				{
					List<PackageInfo> list = e.Argument as List<PackageInfo>;
					Parallel.ForEach<string>(source, (string str) =>
					{
						parser.ReportProgress((progress++ * 100) / progressMax);
						list.Add(PackageInfo.Parse(str));
					});
					e.Result = list;
				};
				parser.ProgressChanged += (object sender, ProgressChangedEventArgs e) => { ProgressBarStatus.Value = e.ProgressPercentage; };
				parser.RunWorkerCompleted += (object sender, RunWorkerCompletedEventArgs e) =>
				{
					Dispatcher.Invoke(new Action(() =>
					{
						foreach (PackageInfo info in e.Result as List<PackageInfo>)
							Packages.Add(info);
					}));
					TextBlockStatus.Text = App.GetResource<string>(Properties.Resources.ResKey_String_Ready);
					ProgressBarStatus.Value = 0;
				};
				Packages.Clear();
				parser.WorkerReportsProgress = true;
				parser.RunWorkerAsync(new List<PackageInfo>());
			}
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
			Height = Properties.Settings.Default.MainWindowHeight;
			Left = Properties.Settings.Default.MainWindowLeft;
			if (Properties.Settings.Default.MainWindowMaximized)
				WindowState = WindowState.Maximized;
			Top = Properties.Settings.Default.MainWindowTop;
			Width = Properties.Settings.Default.MainWindowWidth;
			if (Top == 0 && Left == 0)
				WindowStartupLocation = WindowStartupLocation.CenterOwner;
		}

		#endregion

		private void Window_Closed(object sender, EventArgs e)
		{
			Properties.Settings.Default.MainWindowHeight = Height;
			Properties.Settings.Default.MainWindowLeft = Left;
			Properties.Settings.Default.MainWindowMaximized = WindowState == WindowState.Maximized;
			Properties.Settings.Default.MainWindowTop = Top;
			Properties.Settings.Default.MainWindowWidth = Width;
		}
	}
}
