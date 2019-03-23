using DebianPackagesExplorer.Debian;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

		public string PackagesFileName { get; private set; }

		public SiteInfoCollection SiteInfo { get; }

		public ObservableCollection<string> Sources { get; }

		#endregion

		#region Methods

		private void CommandAdd_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = !string.IsNullOrEmpty(ComboBoxSources.Text) && !Sources.Contains(ComboBoxSources.Text);
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
			PackagesFileName = (TreeViewBrowser.SelectedItem as ComponentInfo).Url;
			Properties.Settings.Default.Sources.Clear();
			Properties.Settings.Default.Sources.AddRange(Sources.ToArray());
			DialogResult = true;
		}

		private void CommandRefresh_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = !string.IsNullOrEmpty(ComboBoxSources.Text) && !IsRefreshing;
		}

		private void CommandRefresh_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			string url = string.Format("{0}/README", ComboBoxSources.SelectedItem);
			try
			{

				WebRequest webRequest = WebRequest.Create(url);
				{
					using (Stream stream = webRequest.GetResponse().GetResponseStream())
					using (StreamReader reader = new StreamReader(stream))
					{
						string data = reader.ReadToEnd();
						MatchCollection matches = Regex.Matches(data, @"(?<name>.*) +is(?<distribution>.*) +(?<version>[0-9.]+)");
						if (matches.Count > 0)
						{
							IsRefreshing = true;
							SiteInfo.Clear();
							string baseUrl = ComboBoxSources.SelectedItem as string;
							List<CodeNameInfo> discovery = new List<CodeNameInfo>();
							Task.Factory.StartNew(new Action(() =>
							{
								foreach (Match match in matches)
									discovery.Add(CodeNameInfo.Parse(match, baseUrl));
							})).ContinueWith((task) =>
							{
								Dispatcher.Invoke(new Action(() => {
									foreach (CodeNameInfo info in discovery)
										SiteInfo.Add(info);
									IsRefreshing = false;
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
			catch(Exception ex)
			{
				LabelNothingFound.Content = ex.Message;
				LabelNothingFound.Visibility = Visibility.Visible;
			}
		}

		private void CommandRemove_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = Sources.Contains(ComboBoxSources.SelectedItem as string);
		}

		private void CommandRemove_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Sources.Remove(ComboBoxSources.SelectedItem as string);
		}

		private void TreeViewBrowser_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			CommandOk.Execute(null, null);
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
