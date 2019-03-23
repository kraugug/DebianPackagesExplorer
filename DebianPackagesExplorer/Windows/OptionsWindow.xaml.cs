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
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window
    {
		#region Fileds

		public static readonly RoutedCommand CommandOk = new RoutedCommand();

		#endregion

		#region Methods

		private void CommandOk_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			DialogResult = true;
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
