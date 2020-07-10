using System;
using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;

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
			try
			{
				_webElement.Click();
				return this;
			}
			catch ( StaleElementReferenceException )
			{
				throw new StaleElementException();
			}
			catch ( WebDriverException )
			{
				return Click();
			}
		}

		public IXenonElement SelectDropdownItem( string name )
		{
			new SelectElement( _webElement ).SelectByText( name );
			return this;
		}

		public IXenonElement RightClick()
		{
			try
			{
				const string script = "var evt = document.createEvent('MouseEvents'); evt.initMouseEvent('contextmenu',true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0,null); arguments[0].dispatchEvent(evt);";

				_webDriver.ExecuteJavaScript( script, _webElement );
				return this;
			}
			catch ( StaleElementReferenceException )
			{
				throw new StaleElementException();
			}
			catch ( WebDriverException )
			{
				return RightClick();
			}
		}

		public IXenonElement EnterText( string value )
		{
			if ( _webElement.GetAttribute( "type" ) == "date" )
				throw new IncorrectInputElementTypeException( $"Do not use {nameof( EnterText )} to set DatePicker values, " +
				                                              $"use {nameof( EnterDate )} instead" );

			try
			{
				_webElement.SendKeys( value );
				return this;
			}
			catch ( StaleElementReferenceException )
			{
				throw new StaleElementException();
			}
			catch ( WebDriverException )
			{
				return EnterText( value );
			}
		}

		public bool IsVisible
		{
			get
			{
				try
				{
					return _webElement.Displayed;
				}
				catch ( StaleElementReferenceException )
				{
					throw new StaleElementException();
				}
				catch ( WebDriverException )
				{
					return IsVisible;
				}
			}
		}

		public string Text
		{
			get
			{
				try
				{
					if ( _webElement.TagName == "input" || _webElement.TagName == "textarea" )
						return _webElement.GetAttribute( "value" );
					return _webElement.Text;
				}
				catch ( StaleElementReferenceException )
				{
					throw new StaleElementException();
				}
				catch ( WebDriverException )
				{
					return Text;
				}
			}
		}

		public IXenonElement Clear()
		{
			try
			{
				_webElement.Clear();
				return this;
			}
			catch ( StaleElementReferenceException )
			{
				throw new StaleElementException();
			}
			catch ( WebDriverException )
			{
				return Clear();
			}
		}

		public IXenonElement ScrollToElement()
		{
			try
			{
				_webDriver.ExecuteJavaScript( "arguments[0].scrollIntoView(true);", _webElement );
				return this;
			}
			catch ( StaleElementReferenceException )
			{
				throw new StaleElementException();
			}
			catch ( WebDriverException )
			{
				return ScrollToElement();
			}
		}

		public IXenonElement EnterDate( DateTime date, string dateFormat = XenonTestOptions.DefaultDateFormat )
		{
			_webElement.SendKeys( date.ToString( dateFormat ) );
			return this;
		}

		public IXenonElement MoveToElement()
		{
			try
			{
				new Actions( _webDriver )
					.MoveToElement( _webElement )
					.Build()
					.Perform();

				return this;
			}
			catch ( StaleElementReferenceException )
			{
				throw new StaleElementException();
			}
			catch ( WebDriverException )
			{
				return MoveToElement();
			}
		}
	}
}