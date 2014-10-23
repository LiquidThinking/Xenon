using System;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;

namespace Xenon.Selenium
{
	public class XenonTest
	{
		public static XenonTestOptions XenonTestOptions { get; set; }

		private readonly XenonTestOptions _xenonTestOptions;
		private readonly IXenonBrowser _browser;

		public XenonTest( IXenonBrowser browser )
			: this( browser, XenonTestOptions ?? new XenonTestOptions() ) {}

		public XenonTest( IXenonBrowser browser, XenonTestOptions options )
		{
			_xenonTestOptions = options;
			_browser = browser;
		}


		private void WaitUntil( Func<XenonAssertion, XenonAssertion> wait )
		{
			var endTime = DateTime.Now.AddSeconds( _xenonTestOptions.WaitForSeconds );

			do
			{
				if ( _browser.RunAssertion( wait ).Passing )
					break;

				Thread.Sleep( 10 );
			} while ( DateTime.Now < endTime );
		}

		private XenonTest RunTask( Action<IXenonBrowser> task, Func<XenonAssertion, XenonAssertion> preWait = null, Func<XenonAssertion, XenonAssertion> postWait = null )
		{
			if ( preWait != null )
				WaitUntil( preWait );

			task( _browser );

			if ( postWait != null )
				WaitUntil( postWait );

			return this;
		}

		public XenonTest GoToUrl( string url, Func<XenonAssertion, XenonAssertion> customPostWait = null )
		{
			return RunTask( w => w.GoToUrl( url ), postWait: customPostWait ?? ( a => a.UrlContains( url ) ) );
		}

		public XenonTest Click( string cssSelector, Func<XenonAssertion, XenonAssertion> customPreWait = null )
		{
			return RunTask( w => w.FindElementsByCssSelector( cssSelector ).First().Click(), customPreWait ?? ( a => a.ContainsElement( cssSelector ) ) );
		}

		public XenonTest EnterText( string cssSelector, string text, Func<XenonAssertion, XenonAssertion> customPreWait = null )
		{
			return RunTask( w => w.FindElementsByCssSelector( cssSelector ).First().SetValue( text ), customPreWait ?? ( a => a.ContainsElement( cssSelector ) ) );
		}

		public void Test( Action<bool, string> testFrameworkIsTrue, Func<XenonAssertion, XenonAssertion> assertion )
		{
			WaitUntil( assertion );
			var assertionResult = assertion( new XenonAssertion( _browser ) );
			testFrameworkIsTrue( assertionResult.Passing, string.Join( "\r\n", assertionResult.FailureMessages ) );
		}
	}
}