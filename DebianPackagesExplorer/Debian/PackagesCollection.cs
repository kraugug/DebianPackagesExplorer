/*
 * Copyright(C) 2019, Michal Heczko All rights reserved.
 *
 * This software may be modified and distributed under the terms of the
 * GNU General Public License v3.0. See the LICENSE file for details.
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace DebianPackagesExplorer.Debian
{
	public class PackagesCollection : ObservableCollection<PackageInfo>
	{
		#region Constants

		public const string PackageInfoDelimiter = "\n\n";

		#endregion

		#region Properties

		public SelectedSourcePackageInfo SourceInfo { get; set; }

		#endregion

		#region Methods

		public new void Add(PackageInfo item)
		{
			if (item != null)
				base.Add(item);
			else
				System.Diagnostics.Debug.WriteLine("PackagesCollection.Add <= {0}", new object[] { item });
		}

		public string GetPackageDownloadLink(PackageInfo package)
		{
			if (string.IsNullOrEmpty(SourceInfo?.Url) || package == null)
				return null;
			return string.Format("{0}/{1}", SourceInfo.BaseUrl, package.FileNameWithPath);
		}

		public static IEnumerable<string> ParseString(string str)
		{
			using (StringReader reader = new StringReader(str))
				return reader.ReadToEnd().Split(new string[] { PackageInfoDelimiter }, StringSplitOptions.None);
		}

		public static IEnumerable<string> ParseArchive(string filename)
		{
			using (FileStream stream = new FileStream(filename, FileMode.Open))
				return ParseCompressedStream(stream);
		}
		
		public static IEnumerable<string> ParseCompressedStream(Stream stream)
		{
			return ParseRawStream(new GZipStream(stream, CompressionMode.Decompress));
		}		

		public static IEnumerable<string> ParseRawStream(Stream stream)
		{
			using (StreamReader reader = new StreamReader(stream))
			{
				string data = reader.ReadToEnd();
				if (string.IsNullOrEmpty(data))
					throw new InvalidDataException(App.GetResource<string>(Properties.Resources.ResKey_String_EmptyPackagesFile));
				return data.Split(new string[] { PackageInfoDelimiter }, StringSplitOptions.None);
			}
		}

		public static IEnumerable<string> ParseTextFile(string filename)
		{
			using (FileStream stream = new FileStream(filename, FileMode.Open))
				return ParseRawStream(stream);
		}

		public void ParseList(IEnumerable<string> source, bool clear = true)
		{
			if (clear)
				Clear();
			foreach (var item in source)
				Add(PackageInfo.Parse(item));
		}

		public void ParseListParallel(IEnumerable<string> source, bool clear = true)
		{
			if (clear)
				Clear();
			PackagesCollection collection = new PackagesCollection();
			Parallel.ForEach(source, (string str) => collection.Add(PackageInfo.Parse(str)));
			foreach (var item in collection)
				Add(item);
		}


		#endregion

		#region Constructor

		public PackagesCollection()
		{ }

		public PackagesCollection(IEnumerable<string> source) : this()
		{
			foreach (string item in source)
				Add(PackageInfo.Parse(item));
		}

		#endregion

		#region Indexers

		public PackageInfo this[string name]
		{
			get { return this.Where(p => p != null && p.Name.Contains(name)).FirstOrDefault(); }
		}

		#endregion
	}
}
