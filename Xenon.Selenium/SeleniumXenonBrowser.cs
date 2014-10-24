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

		public string Url => _driver.Url;

		public string PageSource => _driver.PageSource;

		public IEnumerable<IXenonElement> FindElementsByCssSelector( string cssSelector ) => _driver.FindElementsByCssSelector( cssSelector ).Select( ConvertToXenonElement );

		public void GoToUrl( string url ) => _driver.Navigate().GoToUrl( url );

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

		private IXenonElement ConvertToXenonElement( IWebElement webElement ) => new SeleniumXenonElement( webElement );
	}
}