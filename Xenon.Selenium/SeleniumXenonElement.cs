using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Xenon.Selenium
{
	public class SeleniumXenonElement : IXenonElement
	{
		private readonly IWebDriver _webDriver;
		private readonly IWebElement _webElement;

		public SeleniumXenonElement( IWebDriver webDriver, IWebElement webElement )
		{
			_webDriver = webDriver;
			_webElement = webElement;
		}

		public IXenonElement Click()
		{
			_webElement.Click();
			return this;
		}

		public IXenonElement EnterText( string value )
		{
			_webElement.SendKeys( value );
			return this;
		}

		public bool IsVisible
		{
			get { return _webElement.Displayed; }
		}

		public string Text
		{
			get
			{
				if ( _webElement.TagName == "input" || _webElement.TagName == "textarea" )
					return _webElement.GetAttribute( "value" );
				return _webElement.Text;
			}
		}

		public IXenonElement Clear()
		{
			_webElement.Clear();
			return this;
		}

		public IXenonElement ScrollToElement()
		{
			var actions = new Actions( _webDriver );
			actions.MoveToElement( _webElement ).Perform();
			return this;
		}
	}
}