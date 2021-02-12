using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace learningdotnet
{
	class Site
	{
		public string URL { get; set; } = "";
		public string SiteData { get; set; } = "";

		public long Ping { get; set; } = 0;

		public Site() { }

		public Site(string url, string data)
		{
			URL = url;
			SiteData = data;
		}
	}

	class Timer
	{
		private Stopwatch watch = new Stopwatch();

		public Timer() => Start();

		public void Start() => watch = Stopwatch.StartNew();

		public long GetElapsedMs() => watch.ElapsedMilliseconds;
	}

	class TestAsync
	{
		public async Task GetSitesAsync()
		{
			List<string> urls = new List<string>();
			List<Task<Site>> sites = new List<Task<Site>>();

			urls.Add("https://www.google.com/");
			urls.Add("https://www.yandex.com/");
			urls.Add("https://www.yahoo.com/");
			urls.Add("https://www.youtube.com/");
			urls.Add("https://www.vk.com/");
			urls.Add("https://www.stackoverflow.com/");

			foreach (var item in urls)
				sites.Add(Task.Run(() => DownloadSiteByURL(item)));

			var results = await Task.WhenAll(sites);

			foreach(var site in results)
			{
				Console.WriteLine($"{site.URL} - {site.SiteData.Length} chars; Ping: {site.Ping}");
			}
		}

		private Site DownloadSiteByURL(string URL)
		{
			Site site = new Site();

			site.URL = URL;
			Timer timer = new Timer();
			site.SiteData = new WebClient().DownloadString(URL);
			site.Ping = timer.GetElapsedMs();

			return site;
		}
	}

	class Program
	{
		static async Task Main(string[] args)
		{
			Timer timer = new Timer();

			await new TestAsync().GetSitesAsync();

			var time = timer.GetElapsedMs();

			Console.WriteLine($"Execution time: {time}ms");
			Console.Read();
		}

		
	}
}

/*
	Output:
	https://www.google.com/ - 52045 chars; Ping: 641
	https://www.yandex.com/ - 154482 chars; Ping: 2331
	https://www.yahoo.com/ - 134346 chars; Ping: 2929
	https://www.youtube.com/ - 479251 chars; Ping: 1779
	https://www.vk.com/ - 29064 chars; Ping: 3100
	https://www.stackoverflow.com/ - 122478 chars; Ping: 2379
	Execution time: 3209ms
*/