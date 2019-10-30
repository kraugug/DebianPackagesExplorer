using DebianPackagesExplorer.Debian;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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
		public static readonly RoutedCommand CommandRemove = new RoutedCommand();

		#endregion

		#region Properties

		public bool IsRefreshing { get; private set; }

		public SelectedSourcePackageInfo SelectedPackagesSource { get; private set; }

		public SiteInfoCollection SiteInfo { get; }

		public ObservableCollection<string> Sources { get; }

		#endregion

		#region Methods

		private void CommandAdd_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = !string.IsNullOrEmpty(ComboBoxSources.Text) && !Sources.Contains(ComboBoxSources.Text) && !IsRefreshing;
		}

		private void CommandAdd_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Sources.Add(ComboBoxSources.Text);
		}

		private void CommandOk_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (TreeViewBrowser.SelectedItem is ComponentInfo) && !IsRefreshing;
		}

		private void CommandOk_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			SelectedPackagesSource = new SelectedSourcePackageInfo(TreeViewBrowser.SelectedItem as ComponentInfo);
			DialogResult = true;
		}

		private void CommandRefresh_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = !string.IsNullOrEmpty(ComboBoxSources.Text) && !IsRefreshing;
		}

		private void CommandRefresh_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			IsRefreshing = true;
			ProgressBarStatus.IsIndeterminate = true;
			ProgressBarStatus.Value = 0;
			string url = string.Format("{0}/README", ComboBoxSources.Text);
			try
			{
				WebRequest request = WebRequest.Create(url);
				{
					using (Stream stream = request.GetResponse().GetResponseStream())
					using (StreamReader reader = new StreamReader(stream))
					{
						string data = reader.ReadToEnd();
						MatchCollection matches = Regex.Matches(data, @"(?<name>.*) +is(?<distribution>.*) +(?<version>[0-9.]+)");
						if (matches.Count > 0)
						{
							SiteInfo.Clear();
							string baseUrl = ComboBoxSources.SelectedItem as string;
							List<CodeNameInfo> discovery = new List<CodeNameInfo>();
							ProgressBarStatus.IsIndeterminate = false;
							Task.Factory.StartNew(new Action(() =>
							{
								Dispatcher.Invoke(new Action(() => { ProgressBarStatus.Maximum = matches.Count; }));								
								foreach (Match match in matches)
								{
									discovery.Add(CodeNameInfo.Parse(match, baseUrl));
									Dispatcher.Invoke(new Action(() => { ProgressBarStatus.Value++; }));
								}
							})).ContinueWith((task) =>
							{
								Dispatcher.Invoke(new Action(() =>
								{
									foreach (CodeNameInfo info in discovery)
										SiteInfo.Add(info);
									CommandManager.InvalidateRequerySuggested();
								}));
							});

							//foreach (Match match in matches)
							//{
							//	CodeNameInfo info = CodeNameInfo.Parse(match, ComboBoxSources.SelectedItem as string);
							//	if (info != null)
							//		SiteInfo.Add(info);
							//}
						}
						else
							LabelNothingFound.Content = App.GetResource<string>(Properties.Resources.ResKey_String_NothingFound);
						LabelNothingFound.Visibility = matches.Count > 0 ? Visibility.Collapsed : Visibility.Visible;
					}
				}
			}
			catch(System.Net.WebException)
			{
				ParseRemoteFolder(ComboBoxSources.Text);
			}
			finally
			{
				ProgressBarStatus.IsIndeterminate = false;
				IsRefreshing = false;
			}
		}

		private void CommandRemove_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = Sources.Contains(ComboBoxSources.SelectedItem as string) && !IsRefreshing;
		}

		private void CommandRemove_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Sources.Remove(ComboBoxSources.SelectedItem as string);
		}

		public void ParseRemoteFolder(string url)
		{
			try
			{
				HtmlWeb web = new HtmlWeb();
				HtmlDocument document = web.Load(url);
				HtmlNode table = document.DocumentNode.SelectNodes("//table").Cast<HtmlNode>().FirstOrDefault();
				if (table != null)
				{
					List<string> folders = new List<string>();
					IEnumerable<HtmlNode> tableRows = table.SelectNodes("tr").Cast<HtmlNode>();
					for (int index = 2; index < tableRows.Count(); index++)
					{
						HtmlNode tableRow = tableRows.ElementAt(index);
						if (tableRow.ChildNodes.Count > 1)
						{
							string item = tableRow.ChildNodes[1].ChildNodes[0].InnerHtml;
							if (item.ToUpper().Contains("DISTS") || item.ToUpper().Contains("-PORTS"))
							{
								folders.Add(item);
							}
						}
					}
					Debug.WriteLine(folders.Count.ToString());
				}

				//FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
				//{
				//	request.Method = WebRequestMethods.Ftp.ListDirectory;
				//	using (Stream stream = request.GetResponse().GetResponseStream())
				//	using (StreamReader reader = new StreamReader(stream))
				//	{
				//		string data = reader.ReadToEnd();
				//	}
				//}
			}
			catch (Exception ex)
			{
				LabelNothingFound.Content = "Nothing found";
				LabelNothingFound.Visibility = Visibility.Visible;
			}
		}

		private void TreeViewBrowser_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			CommandOk.Execute(null, null);
		}

		private void Window_Closed(object sender, EventArgs e)
		{
			Properties.Settings.Default.Sources.Clear();
			Properties.Settings.Default.Sources.AddRange(Sources.ToArray());
		}

		#endregion

		#region Constructor

		public PackagesSourcesWindow()
		{
			DataContext = this;
			IsRefreshing = false;
			Owner = App.MainWindow;
			SiteInfo = new SiteInfoCollection();
			string[] sources = new string[Properties.Settings.Default.Sources.Count];
			Properties.Settings.Default.Sources.CopyTo(sources, 0);
			Sources = new ObservableCollection<string>(sources);
			InitializeComponent();
		}

		#endregion
	}
}
