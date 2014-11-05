using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using Xenon.Selenium;

namespace Xenon.Tests.Intergration
{
    public class BrowserTest : IDisposable
    {
        public Page Page { get; set; }

        private HttpServer _httpServer;

        public BrowserTest(string html)
        {
            Page = new Page { Html = html };
        }

        public IXenonBrowser Start()
        {
            var port = FreeTcpPort();

            _httpServer = new MyHttpServer(port, Page);

            var thread = new Thread(_httpServer.Listen);
            thread.Start();

            return new SeleniumXenonBrowserWrapper(new ChromeDriver(Environment.CurrentDirectory), port);
        }

        int FreeTcpPort()
        {
            TcpListener l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            int port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();
            return port;
        }

        class SeleniumXenonBrowserWrapper : SeleniumXenonBrowser, IXenonBrowser
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
            _httpServer.Dispose();
        }

        public NameValueCollection GetPostResult()
        {
            return _httpServer.GetPostResult();
        }
    }
}