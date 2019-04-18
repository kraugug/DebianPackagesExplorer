using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

		public static readonly RoutedCommand CommandClear = new RoutedCommand();
		public static readonly RoutedCommand CommandOk = new RoutedCommand();

		#endregion

		#region Properties

		public ObservableCollection<string> History { get; }

		public string Link { get; set; }

		#endregion

		#region Methods

		private void CommandClear_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = History.Count > 0;
		}

		private void CommandClear_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			History.Clear();
			Properties.Settings.Default.LinkHistory.Clear();
			Properties.Settings.Default.LinkHistory.AddRange(History.ToArray());
		}

		private void CommandOk_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = !string.IsNullOrEmpty(Link);
		}

		private void CommandOk_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Uri testUri;
			if (Uri.TryCreate(Link, UriKind.Absolute, out testUri))
			{
				DialogResult = true;
			}
			else
				MessageBox.Show(App.GetResource<string>(Properties.Resources.ResKey_String_InvalidLink), Title, MessageBoxButton.OK, MessageBoxImage.Error);
		}

		#endregion

		#region Constructor

		public OpenLinkWindow()
		{
			DataContext = this;
			string[] history = new string[Properties.Settings.Default.LinkHistory.Count];
			Properties.Settings.Default.LinkHistory.CopyTo(history, 0);
			History = new ObservableCollection<string>(history);
			Owner = App.MainWindow;
			InitializeComponent();
		}

		#endregion
	}
}
