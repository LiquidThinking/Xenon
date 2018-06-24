using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.Extensions;

namespace Xenon.Selenium
{
	public class SeleniumXenonBrowser : IXenonBrowser
	{
		private readonly RemoteWebDriver _driver;

		public SeleniumXenonBrowser( RemoteWebDriver driver )
		{
			_driver = driver;
		}

		public string Url => _driver.Url;

		public string PageSource => _driver.PageSource;

		private IXenonElement ConvertToXenonElement( IWebElement webElement )
		{
			return new SeleniumXenonElement( _driver, webElement );
		}

		public XenonElementSearchResult FindElementsByCssSelector( string cssSelector )
		{
			var elements = _driver.FindElementsByCssSelector( cssSelector )
				.Select( ConvertToXenonElement ).ToList();

			return new XenonElementSearchResult(
				elements,
				$"Searching for element(s) with Css Selector '{cssSelector}'" );
		}

		public XenonElementSearchResult FindElementsByXPath( string xpath )
		{
			var elements = _driver.FindElementsByXPath( xpath )
				.Select( ConvertToXenonElement )
				.ToList();

			return new XenonElementSearchResult(
				elements,
				$"Searching for elements with XPath '{xpath}'" );
		}

		public void GoToUrl( string url ) => _driver.Navigate().GoToUrl( url );

		public XenonAssertion RunAssertion( AssertionFunc assertion )
		{
			XenonAssertion result;
			try
			{
				result = assertion( new XenonAssertion( this ) );
			}
			catch ( StaleElementException exception )
			{
				result = new XenonAssertion( this ).BrowserFailure( exception.Message );
			}

			return result;
		}

		public void Quit() => _driver.Quit();

		public IXenonBrowser SwitchToWindow( AssertionFunc assertion )
		{
			foreach ( var windowHandle in _driver.WindowHandles )
			{
				var switchedWindowDriver = _driver.SwitchTo().Window( windowHandle );
				var switchedWindowXenonBrowser = new SeleniumXenonBrowser( (RemoteWebDriver)switchedWindowDriver );
				try
				{
					if ( assertion( new XenonAssertion( switchedWindowXenonBrowser ) ).Passing )
						return switchedWindowXenonBrowser;
				}
				catch ( NoElementsFoundException )
				{
				}
			}

			return new SeleniumXenonBrowser( _driver );
		}

		public IXenonBrowser ClickDialogBox()
		{
			return DialogBoxInteraction( () => _driver.SwitchTo().Alert().Accept() );
		}

		private IXenonBrowser DialogBoxInteraction( Action actionToPerform )
		{
			var tryUntil = DateTime.Now.AddSeconds( XenonTestOptions.Options.WaitForSeconds );
			while ( true )
			{
				try
				{
					actionToPerform();
					return this;
				}
				catch ( Exception )
				{
					if ( DateTime.Now > tryUntil )
						throw;
					Thread.Sleep( 100 );
				}
			}
		}

		public bool DialogBoxIsActive()
		{
			var result = false;
			try
			{
				_driver.SwitchTo().Alert();
				result = true;
			}
			catch ( NoAlertPresentException ) { }
			catch ( UnhandledAlertException ) { }

			return result;
		}

		public IXenonBrowser EnterTextInDialogBox( string text )
		{
			return DialogBoxInteraction( () => _driver.SwitchTo().Alert().SendKeys( text ) );
		}

		public IXenonBrowser CancelDialogBox()
		{
			return DialogBoxInteraction( () => _driver.SwitchTo().Alert().Dismiss() );
		}

		public void TakeScreenshot( string path )
		{
			_driver.TakeScreenshot().SaveAsFile( path, ScreenshotImageFormat.Jpeg );
		}

		public void ClearLocalStorage()
		{
			_driver.ExecuteScript( "try { localStorage.clear(); } catch(ex){} " );
		}

		public void ClearSessionStorage()
		{
			_driver.ExecuteScript( "try { sessionStorage.clear(); } catch(ex){} " );
		}

		public void ClearCookies()
		{
			try
			{
				_driver.Manage().Cookies.DeleteAllCookies();
			}
			catch { }
		}

		public object ExecuteJavascript( string script, params object[] args )
		{
			try
			{
				return _driver.ExecuteScript( script, args );
			}
			catch
			{
				return new object();
			}
		}

		public void CloseWindow() => _driver.Close();

		public void Dispose()
		{
			_driver.Quit();
			_driver.Dispose();
		}
	}
}