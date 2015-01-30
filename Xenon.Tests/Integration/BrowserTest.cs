using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.Owin.Hosting;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using Xenon.Selenium;

namespace Xenon.Tests.Integration
{
	public class BrowserTest : IDisposable
	{
		public Page Page { get; set; }

		private IDisposable _webApp;
		private IXenonBrowser _xenonBrowser;

		public BrowserTest( string html )
		{
			Page = new Page
			{
				Html = html
			};
		}

		public IXenonBrowser Start()
		{
			var port = FreeTcpPort();

			Startup.Html = Page.Html;
			_webApp = WebApp.Start<Startup>( new StartOptions
			{
				ServerFactory = "Nowin",
				Port = port
			} );

			return _xenonBrowser = new SeleniumXenonBrowserWrapper( new ChromeDriver( Environment.CurrentDirectory ), port );
		}

		private int FreeTcpPort()
		{
			TcpListener l = new TcpListener( IPAddress.Loopback, 0 );
			l.Start();
			int port = ( (IPEndPoint)l.LocalEndpoint ).Port;
			l.Stop();
			return port;
		}

		private class SeleniumXenonBrowserWrapper : SeleniumXenonBrowser, IXenonBrowser
		{
			private readonly int _port;

			public SeleniumXenonBrowserWrapper( RemoteWebDriver driver, int port )
				: base( driver )
			{
				_port = port;
			}

			void IXenonBrowser.GoToUrl( string url )
			{
				base.GoToUrl( "http://localhost:" + _port + url );
			}
		}

		public void Dispose()
		{
			_webApp.Dispose();
			if ( _xenonBrowser != null )
				_xenonBrowser.Dispose();
		}

		public NameValueCollection GetPostResult()
		{
			return Startup.GetPostResult();
		}
	}
}