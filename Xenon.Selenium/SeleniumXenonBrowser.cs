using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace Xenon.Selenium
{
	public class SeleniumXenonBrowser : IXenonBrowser
	{
		private readonly RemoteWebDriver _driver;

		public SeleniumXenonBrowser( RemoteWebDriver driver )
		{
			_driver = driver;
		}

		public string Url
		{
			get { return _driver.Url; }
		}

		public string PageSource
		{
			get { return _driver.PageSource; }
		}

		private IXenonElement ConvertToXenonElement( IWebElement webElement )
		{
			return new SeleniumXenonElement( webElement );
		}

		public IEnumerable<IXenonElement> FindElementsByCssSelector( string cssSelector )
		{
			return _driver.FindElementsByCssSelector( cssSelector ).Select( ConvertToXenonElement );
		}

		public IEnumerable<IXenonElement> FindElementsByXPath( string xpath )
		{
			return _driver.FindElementsByXPath( xpath ).Select( ConvertToXenonElement );
		}

		public void GoToUrl( string url )
		{
			_driver.Navigate().GoToUrl( url );
		}

		public XenonAssertion RunAssertion( AssertionFunc assertion )
		{
			XenonAssertion result;
			try
			{
				result = assertion( new XenonAssertion( this ) );
			}
			catch ( StaleElementReferenceException exception )
			{
				result = new XenonAssertion( this ).BrowserFailure( exception.Message );
			}

			return result;
		}

		public void Quit()
		{
			_driver.Quit();
		}

		public IXenonBrowser SwitchToWindow( AssertionFunc assertion )
		{
			foreach ( var windowHandle in _driver.WindowHandles )
			{
				var switchedWindowDriver = _driver.SwitchTo().Window( windowHandle );
				var switchedWindowXenonBrowser = new SeleniumXenonBrowser( (RemoteWebDriver)switchedWindowDriver );
				if ( assertion( new XenonAssertion( switchedWindowXenonBrowser ) ).Passing )
					return switchedWindowXenonBrowser;
			}

			return new SeleniumXenonBrowser( _driver );
		}

		public IXenonBrowser ClickDialogBox()
		{
			_driver.SwitchTo().Alert().Accept();
			return this;
		}

		public bool DialogBoxIsActive()
		{
			bool result = false;
			try
			{
				_driver.SwitchTo().Alert();
				result = true;
			}
			catch ( NoAlertPresentException ) {}
			catch ( UnhandledAlertException ) {}

			return result;
		}

		public IXenonBrowser EnterTextInDialogBox( string text )
		{
			_driver.SwitchTo().Alert().SendKeys( text );
			return this;
		}

		public IXenonBrowser CancelDialogBox()
		{
			_driver.SwitchTo().Alert().Dismiss();
			return this;
		}

		public void CloseWindow()
		{
			_driver.Close();
		}

		public void Dispose()
		{
			_driver.Quit();
			_driver.Dispose();
		}
	}
}