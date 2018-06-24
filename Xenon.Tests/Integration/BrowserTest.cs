using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.Owin.Hosting;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using Xenon.Selenium;

namespace Xenon.Tests.Integration
{
	public class BrowserTest : IDisposable
	{
		public Page Page { get; set; }

		private IDisposable _webApp;
		private IXenonBrowser _xenonBrowser;

		public BrowserTest(string html)
		{
			Page = new Page
			{
				Html = html
			};
		}

		public IXenonBrowser Start( BrowserType browserType = BrowserType.Firefox )
		{
			var port = GetFreeTcpPort();

			Startup.Html = Page.Html;
			_webApp = WebApp.Start<Startup>( new StartOptions
			{
				ServerFactory = "Nowin",
				Port = port,
			} );

			return _xenonBrowser = new SeleniumXenonBrowserWrapper( CreateDriver(), port );

			RemoteWebDriver CreateDriver()
			{
				var firefoxOptions = new FirefoxOptions();
				firefoxOptions.AddArgument( "--headless" );
				switch ( browserType )
				{
					case BrowserType.Chrome:
						return new ChromeDriver( Environment.CurrentDirectory );
					case BrowserType.Firefox:
						return new FirefoxDriver( Environment.CurrentDirectory, firefoxOptions );
					default:
						throw new IndexOutOfRangeException();
				}
			}
		}

		private int GetFreeTcpPort()
		{
			var tcpListener = new TcpListener( IPAddress.Loopback, 0 );
			tcpListener.Start();
			var port = ( (IPEndPoint)tcpListener.LocalEndpoint ).Port;
			tcpListener.Stop();
			return port;
		}

		private class SeleniumXenonBrowserWrapper : SeleniumXenonBrowser, IXenonBrowser
		{
			private readonly int _port;

			public SeleniumXenonBrowserWrapper(RemoteWebDriver driver, int port)
				: base(driver)
			{
				_port = port;
			}

			void IXenonBrowser.GoToUrl(string url)
			{
				base.GoToUrl("http://localhost:" + _port + url);
			}
		}

		public void Dispose()
		{
			_webApp.Dispose();
			_xenonBrowser?.Dispose();
		}

		public NameValueCollection GetPostResult()
		{
			return Startup.GetPostResult();
		}
	}

	public enum BrowserType
	{
		Chrome,
		Firefox
	}
}