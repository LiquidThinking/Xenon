using OpenQA.Selenium;

namespace Xenon.Selenium
{
	public class SeleniumXenonElement : IXenonElement
	{
		private readonly IWebElement _webElement;

		public SeleniumXenonElement( IWebElement webElement )
		{
			_webElement = webElement;
		}

		public void Click()
		{
			_webElement.Click();
		}

		public void EnterText( string value )
		{
			_webElement.SendKeys( value );
		}
	}
}