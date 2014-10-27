using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

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
			get
			{
				return _driver.Url;
			}
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
	}
}