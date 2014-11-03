using System;
using System.Linq;
using System.Threading;

namespace Xenon
{
	public abstract class BaseXenonTest<T> where T : BaseXenonTest<T>
	{
		protected readonly XenonTestOptions _xenonTestOptions;
		protected readonly IXenonBrowser _xenonBrowser;

		public BaseXenonTest( IXenonBrowser xenonBrowser ) : this( xenonBrowser, XenonTestOptions.Options ?? new XenonTestOptions() ) {}

		public BaseXenonTest( IXenonBrowser browser, XenonTestOptions options )
		{
			_xenonTestOptions = options;
			_xenonBrowser = browser;
		}

		private void WaitUntil( AssertionFunc wait )
		{
			var endTime = DateTime.Now.AddSeconds( _xenonTestOptions.WaitForSeconds );

			do
			{
				if ( _xenonBrowser.RunAssertion( wait ).Passing )
					break;

				Thread.Sleep( 10 );
			} while ( DateTime.Now < endTime );
		}

		private T RunTask( Action<IXenonBrowser> task, AssertionFunc preWait, AssertionFunc postWait )
		{
			if ( preWait != null )
				WaitUntil( preWait );

			task( _xenonBrowser );

			if ( postWait != null )
				WaitUntil( postWait );

			return this as T;
		}

		/// <summary>
		/// Goes to the url specified.
		/// By default waits for the page to change to the specified url
		/// </summary>
		/// <param name="url">The url you want to goto, must be absolute</param>
		/// <param name="customPreWait">Custom action wait upon before going to the url</param>
		/// <param name="customPostWait">Custom action wait upon after going to the url</param>
		public T GoToUrl( string url, AssertionFunc customPreWait = null, AssertionFunc customPostWait = null )
		{
			return RunTask( w => w.GoToUrl( url ),
			                customPreWait,
			                customPostWait ?? ( a => a.UrlContains( url ) ) );
		}

		/// <summary>
		/// Clicks the element specified
		/// By default waits for the element to exist before clicking
		/// </summary>
		/// <param name="cssSelector">The css selector of the element</param>
		/// <param name="customPreWait">Custom action wait upon before clicking to the element</param>
		/// <param name="customPostWait">Custom action wait upon after clicking to the element</param>
		public T Click( string cssSelector, AssertionFunc customPreWait = null, AssertionFunc customPostWait = null )
		{
			return RunTask( browser => browser.FindElementsByCssSelector( cssSelector ).First().Click(),
			                customPreWait ?? ( a => a.ContainsElement( cssSelector ) ),
			                customPostWait );
		}


		/// <summary>
		/// Clicks the element specified
		/// By default waits for the element to exist before clicking
		/// </summary>
		/// <param name="where">element finder function</param>
		/// <param name="customPreWait">Custom action wait upon before clicking to the element</param>
		/// <param name="customPostWait">Custom action wait upon after clicking to the element</param>
		public T Click( Func<XenonElementsFinder, XenonElementsFinder> where, AssertionFunc customPreWait = null, AssertionFunc customPostWait = null )
		{
			return RunTask( browser => where( new XenonElementsFinder( browser ) ).FindElements().First().Click(),
			                customPreWait ?? ( a => a.ContainsElement( where ) ),
			                customPostWait );
		}


		/// <summary>
		/// Enters the text into the element specified.
		/// By default waits for the element to exist before entering the text
		/// </summary>
		/// <param name="cssSelector">The css selector of the element</param>
		/// <param name="text">The text to insert</param>
		/// <param name="customPreWait">Custom action wait upon before entering the text in the element</param>
		/// <param name="customPostWait">Custom action wait upon after entering the text in the element</param>
		public T EnterText( string cssSelector, string text, AssertionFunc customPreWait = null, AssertionFunc customPostWait = null )
		{
			return RunTask( browser => browser.FindElementsByCssSelector( cssSelector ).First().EnterText( text ),
			                customPreWait ?? ( a => a.ContainsElement( cssSelector ) ),
			                customPostWait );
		}

		/// <summary>
		/// Asserts
		/// </summary>
		/// <param name="assertion">The Function with your assertions</param>
		/// <returns></returns>
		public T Assert( AssertionFunc assertion )
		{
			WaitUntil( assertion );
			var assertionResult = assertion( new XenonAssertion( _xenonBrowser ) );
			_xenonTestOptions.AssertMethod( assertionResult.Passing, string.Join( "\r\n", assertionResult.FailureMessages ) );
			return this as T;
		}
	}
}