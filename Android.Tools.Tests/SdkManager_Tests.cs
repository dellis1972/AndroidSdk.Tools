using System;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Android.Tools.Tests
{
	public class SdkManager_Tests : TestsBase
	{
		public SdkManager_Tests(ITestOutputHelper outputHelper)
			: base(outputHelper)
		{ }

		[Fact]
		public void DownloadSdk()
		{
			var androidSdkHome = new DirectoryInfo(Path.GetTempPath());

			var s = new SdkManager(androidSdkHome);
			s.DownloadSdk(progressHandler: p => OutputHelper.WriteLine($"Downloading SDK {p}%"));

			var isUpToDate = s.IsUpToDate();

			Assert.True(isUpToDate);
		}

		[Fact]
		public void List()
		{
			var s = GetSdkManager();

			var list = s.List();

			Assert.NotNull(list);

			foreach (var a in list.AvailablePackages)
				Console.WriteLine($"{a.Description}\t{a.Version}\t{a.Path}");

			foreach (var a in list.InstalledPackages)
				Console.WriteLine($"{a.Description}\t{a.Version}\t{a.Path}");
		}

		[Fact]
		public void Install()
		{
			var s = GetSdkManager();

			var ok = s.Install("system-images;android-26;google_apis;x86", "extras;google;auto");

			Assert.True(ok);

			var list = s.List();

			Assert.NotNull(list.InstalledPackages.FirstOrDefault(ip => ip.Path == "extras;google;auto"));
		}

		[Fact]
		public void AcceptLicense()
		{
			var s = GetSdkManager();
			s.AcceptLicenses();

			var list = s.List();

			Assert.NotNull(list.InstalledPackages);
		}
	}
}
