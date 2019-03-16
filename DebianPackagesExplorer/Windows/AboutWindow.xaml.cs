using DebianPackagesExplorer.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace DebianPackagesExplorer.Windows
{
	/// <summary>
	/// Interaction logic for AboutWindow.xaml
	/// </summary>
	public partial class AboutWindow : Window
	{
		#region Fields

		public static readonly RoutedCommand CommandCopyInfo = new RoutedCommand();
		public static readonly RoutedCommand CommandSystemInfo = new RoutedCommand();
		public static readonly RoutedCommand CommandOk = new RoutedCommand();
		public static readonly RoutedCommand CommandOpenLicenseFile = new RoutedCommand();

		#endregion

		#region Properties

		public string ApplicationDescription { get { return Assembly.GetExecutingAssembly().GetAttributeValue(AssemblyExtensions.AssemblyAttribute.Description); } }

		public string ApplicationVersionInfo
		{
			get
			{
				Assembly assembly = Assembly.GetExecutingAssembly();
				AssemblyName assemblyName = Assembly.GetExecutingAssembly().GetName();
				string versionInfo = string.Format(Application.Current.Resources[Properties.Resources.ResKey_String_VersionInfo] as string,
					assembly.GetAttributeValue(AssemblyExtensions.AssemblyAttribute.Title), assemblyName.Version, assembly.GetAttributeValue(AssemblyExtensions.AssemblyAttribute.Copyright),
					Application.Current.Resources[Properties.Resources.ResKey_String_AllRightsReserved]);
				return versionInfo.Replace(@"\n", Environment.NewLine);
			}
		}

		public string DotNetVersionInfo
		{
			get
			{
				/*
				Assembly assembly = Assembly.GetExecutingAssembly();
				AssemblyName assemblyName = Assembly.GetExecutingAssembly().GetName();
                string versionInfo = string.Format(App.Current.Resources[Properties.Resources.ResKey_String_DotNetVersionInfo] as string, assembly.ImageRuntimeVersion,
                    App.Current.Resources[Properties.Resources.ResKey_String_AllRightsReserved]);
				*/
				string versionInfo = string.Format(App.Current.Resources[Properties.Resources.ResKey_String_DotNetVersionInfo] as string, "4.0",
					App.Current.Resources[Properties.Resources.ResKey_String_AllRightsReserved]);
				return versionInfo.Replace(@"\n", Environment.NewLine);
			}
		}

		public bool IsLicenseFileExists { get { return System.IO.File.Exists(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Properties.Resources.FileName_License)); } }

		public List<string> LoadedAssemblies
		{
			get
			{
				List<string> assemblies = new List<string>();
				foreach (var assembly in Assembly.GetExecutingAssembly().GetReferencedAssemblies())
					assemblies.Add(assembly.ToString());
				assemblies.Sort();
				return assemblies;
			}
		}

		#endregion

		#region Methods

		private void CommandCopyInfo_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			StringBuilder builder = new StringBuilder();
			builder.AppendLine(ApplicationVersionInfo);
			builder.AppendLine();
			builder.AppendLine(DotNetVersionInfo);
			builder.AppendLine();
			builder.AppendLine(App.Current.Resources[Properties.Resources.ResKey_AboutWindow_GroupBox_LoadedAssemblies_Header] as string);
			foreach (var line in LoadedAssemblies)
				builder.AppendLine(line);
			Clipboard.SetText(builder.ToString());
		}

		private void CommandSystemInfo_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			System.Diagnostics.Process.Start(Properties.Resources.FileName_MSInfo);
		}

		private void CommandOk_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			DialogResult = true;
		}

		private void CommandCommandOpenLicenseFile_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			System.Diagnostics.Process.Start(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Properties.Resources.FileName_License));
		}

		#endregion

		#region Constructor

		public AboutWindow()
		{
			DataContext = this;
			Owner = App.MainWindow;
			InitializeComponent();
			Title = string.Format(App.Current[Properties.Resources.ResKey_AboutWindow_Title] as string, ApplicationDescription);
		}

		#endregion
	}
}
