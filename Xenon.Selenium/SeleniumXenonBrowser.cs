using System;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.Extensions;

namespace Xenon.Selenium
{
	public class SeleniumXenonBrowser : IXenonBrowser
	{
		private readonly WebDriver _driver;

		public SeleniumXenonBrowser( WebDriver driver )
		{
			_driver = driver;
		}

		public string Url => _driver.Url;

		public string PageSource => _driver.PageSource;

		private IXenonElement ConvertToXenonElement( IWebElement webElement )
		{
			return new SeleniumXenonElement( _driver, webElement );
		}

		public XenonElementsSearchResult FindElementsByCssSelector( string cssSelector )
		{
			var elements = _driver.FindElements( By.CssSelector( cssSelector ) )
				.Select( ConvertToXenonElement ).ToList();

			return new XenonElementsSearchResult(
				elements,
				$"Searching for element(s) with Css Selector '{cssSelector}'" );
		}

		public XenonElementsSearchResult FindElementsByXPath( string xpath )
		{
			var elements = _driver.FindElements( By.XPath( xpath ) )
				.Select( ConvertToXenonElement )
				.ToList();

			return new XenonElementsSearchResult(
				elements,
				$"Searching for element(s) with XPath '{xpath}'" );
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
			return SwitchToWindowWithRetries( assertion, 0 );
		}

		private IXenonBrowser SwitchToWindowWithRetries( AssertionFunc assertion, int attemptNumber )
		{
			const int maxTries = 5;

			foreach ( var windowHandle in _driver.WindowHandles )
			{
				var switchedWindowDriver = _driver.SwitchTo().Window( windowHandle );
				var switchedWindowXenonBrowser = new SeleniumXenonBrowser( (RemoteWebDriver)switchedWindowDriver );

				if ( assertion( new XenonAssertion( switchedWindowXenonBrowser ) ).Passing )
					return switchedWindowXenonBrowser;
			}

			return attemptNumber > maxTries
				? new SeleniumXenonBrowser( _driver )
				: SwitchToWindowWithRetries( assertion, ++attemptNumber );
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
			catch ( NoAlertPresentException )
			{
			}
			catch ( UnhandledAlertException )
			{
			}

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
			catch
			{
			}
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
			//no need to call Quit here, because all it does it calls Dispose & is not virtual
			//https://github.com/SeleniumHQ/selenium/blob/master/dotnet/src/webdriver/Remote/RemoteWebDriver.cs#L457
			//Moreover, calling Dispose twice in quick succession takes a long time
			_driver.Dispose();
		}
	}
}