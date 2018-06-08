using System;
using System.Linq;
using System.Threading;

namespace Xenon
{
    public abstract class BaseXenonTest<T> where T : BaseXenonTest<T>
	{
		protected readonly XenonTestOptions _xenonTestOptions;
		protected IXenonBrowser _xenonBrowser;

		protected BaseXenonTest( IXenonBrowser xenonBrowser ) : this( xenonBrowser, XenonTestOptions.Options ?? new XenonTestOptions() ) {}

		protected BaseXenonTest( IXenonBrowser browser, XenonTestOptions options )
		{
			_xenonTestOptions = options;
			_xenonBrowser = browser;
		}

		private void WaitUntil( AssertionFunc wait )
		{
			var endTime = DateTime.Now.AddSeconds( _xenonTestOptions.WaitForSeconds );

			do
			{
				try
				{
					if ( _xenonBrowser.RunAssertion( wait ).Passing )
						break;

				}
				catch ( Exception )
				{
				}

				Thread.Sleep( 10 );
			} while ( DateTime.Now < endTime );
		}

		private T RunTask( Action<IXenonBrowser> task, AssertionFunc preWait, AssertionFunc postWait )
		{
			if ( preWait != null )
				WaitUntil( preWait );

		    try
		    {
		        task( _xenonBrowser );
		    }
            catch( StaleElementException )
		    {
		        return RunTask( task, preWait, postWait );
		    }

		    if ( postWait != null )
				WaitUntil( postWait );

			return this as T;
		}

		/// <summary>
		/// Goes to the url specified.
		/// </summary>
		/// <param name="url">The url you want to goto, must be absolute</param>
		/// <param name="customPreWait">Custom action wait upon before going to the url</param>
		/// <param name="customPostWait">Custom action wait upon after going to the url</param>
		public T GoToUrl( string url, AssertionFunc customPreWait = null, AssertionFunc customPostWait = null )
		{
			return RunTask( w => w.GoToUrl( url ), customPreWait, customPostWait );
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
			return RunTask( browser => browser.FindElementsByCssSelector( cssSelector ).LocateFirstVisibleElement().Click(),
				customPreWait ?? ( a => a.CustomAssertion( browser => browser.FindElementsByCssSelector( cssSelector ).LocateFirstVisibleElement().IsVisible ) ),
				customPostWait );
		}

		/// <summary>
		/// Right clicks the element specified
		/// By default waits for the element to exist before clicking
		/// </summary>
		/// <param name="cssSelector">The css selector of the element</param>
		/// <param name="customPreWait">Custom action wait upon before clicking to the element</param>
		/// <param name="customPostWait">Custom action wait upon after clicking to the element</param>
		public T RightClick( string cssSelector, AssertionFunc customPreWait = null, AssertionFunc customPostWait = null )
		{
			return RunTask( browser => browser.FindElementsByCssSelector( cssSelector ).LocateFirstVisibleElement().RightClick(),
				customPreWait ?? ( a => a.CustomAssertion( browser => browser.FindElementsByCssSelector( cssSelector ).LocateFirstVisibleElement().IsVisible ) ),
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
			return RunTask( browser => where( new XenonElementsFinder( browser ) ).FindElements().LocateSingleVisibleElement().Click(),
				customPreWait ?? ( a => a.CustomAssertion( b => where( new XenonElementsFinder( b ) ).FindElements().LocateSingleVisibleElement().IsVisible ) ),
				customPostWait );
		}

		/// <summary>
		/// Right clicks the element specified
		/// By default waits for the element to exist before clicking
		/// </summary>
		/// <param name="cssSelector">The css selector of the element</param>
		/// <param name="customPreWait">Custom action wait upon before clicking to the element</param>
		/// <param name="customPostWait">Custom action wait upon after clicking to the element</param>
		public T RightClick( Func<XenonElementsFinder, XenonElementsFinder> where, AssertionFunc customPreWait = null, AssertionFunc customPostWait = null )
		{
			return RunTask( browser => where( new XenonElementsFinder( browser ) ).FindElements().LocateSingleVisibleElement().RightClick(),
				customPreWait ?? ( a => a.CustomAssertion( b => where( new XenonElementsFinder( b ) ).FindElements().LocateSingleVisibleElement().IsVisible ) ),
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
			return RunTask( browser => {
								var textInputElement = browser.FindElementsByCssSelector( cssSelector ).LocateFirstVisibleElement();
								textInputElement.EnterText( text );
							},
				customPreWait ?? ( a => a.CustomAssertion( b => b.FindElementsByCssSelector( cssSelector ).LocateFirstVisibleElement().IsVisible ) ),
				customPostWait );
		}

		public T EnterDate( string cssSelector, DateTime date, AssertionFunc preWait = null, AssertionFunc postWait = null )
		{
			return RunTask( browser => {
								var dateInputElement = browser.FindElementsByCssSelector( cssSelector ).LocateFirstVisibleElement();
								dateInputElement.EnterDate( date );
							}, preWait, postWait );
		}

		/// <summary>
		/// Clears text in element
		/// By default waits for the element to exist before entering the text
		/// </summary>
		/// <param name="cssSelector">The css selector of the element</param>
		/// <param name="customPreWait">Custom action wait upon before entering the text in the element</param>
		/// <param name="customPostWait">Custom action wait upon after entering the text in the element</param>
		public T Clear( string cssSelector, AssertionFunc customPreWait = null, AssertionFunc customPostWait = null )
		{
			return RunTask( browser => browser.FindElementsByCssSelector( cssSelector ).LocateFirstVisibleElement().Clear(),
				customPreWait ?? ( a => a.CustomAssertion( b => b.FindElementsByCssSelector( cssSelector ).LocateFirstVisibleElement().IsVisible ) ),
				customPostWait );
		}

		/// <summary>
		/// Selects an element in a dropdown list with specified text
		/// </summary>
		/// <param name="cssSelector">The css selector of the element</param>
		/// <param name="text">The text of the option to select</param>
		/// <param name="customPreWait">Custom action wait upon before selecting the option</param>
		/// <param name="customPostWait">Custom action wait upon after selecting the option</param>
		/// <returns></returns>
		public T SelectList( string cssSelector, string text, AssertionFunc customPreWait = null, AssertionFunc customPostWait = null )
		{
			return RunTask( browser => browser.FindElementsByCssSelector( cssSelector ).First().SelectDropdownItem( text ),
				customPreWait ?? ( a => SelectListPreWait( a, cssSelector, text ) ),
				customPostWait );
		}

		private XenonAssertion SelectListPreWait( XenonAssertion xenonAssertion, string cssSelector, string text )
		{
			if ( xenonAssertion.ContainsElement( cssSelector ).Passing )
			{
				Click( cssSelector );

				xenonAssertion.CustomAssertion(
					browser => browser.FindElementsByCssSelector( cssSelector + " option" ).Any( x => x.Text == text ) );
			}
			return xenonAssertion;
		}

		/// <summary>
		/// Asserts
		/// </summary>
		/// <param name="assertion">The Function with your assertions</param>
		/// <returns></returns>
		public T Assert( AssertionFunc assertion, String message = "" )
		{
			WaitUntil( assertion );
			var assertionResult = assertion( new XenonAssertion( _xenonBrowser ) );
			if ( String.IsNullOrEmpty( message ) )
				message = string.Join( "\r\n", assertionResult.FailureMessages );
			_xenonTestOptions.AssertMethod( assertionResult.Passing, message );
			return this as T;
		}

		/// <summary>
		/// Switch To another window in the browser
		/// </summary>
		/// <param name="assertion">Assertion function to find the window</param>
		/// <param name="customPreWait">Custom action wait upon before switching to another window</param>
		/// <param name="customPostWait">Custom action wait upon after switching to another window</param>
		/// <returns></returns>
		public T SwitchToWindow( AssertionFunc assertion, AssertionFunc customPreWait = null, AssertionFunc customPostWait = null )
		{
			return RunTask( b => _xenonBrowser = b.SwitchToWindow( assertion ), customPreWait, customPostWait );
		}

		/// <summary>
		/// Close current active window and switch to another window
		/// </summary>
		/// <param name="switchToWindowAssertFunc">Assertion function to find the window</param>
		/// <param name="customPreWait">Custom action wait upon before closing and switching to another window</param>
		/// <param name="customPostWait">Custom action wait upon after closing and switching to another window</param>
		/// <returns></returns>
		public T CloseCurrentAndSwitchToWindow( AssertionFunc switchToWindowAssertFunc, AssertionFunc customPreWait = null, AssertionFunc customPostWait = null )
		{
			return RunTask( b =>
			{
				b.CloseWindow();
				b.SwitchToWindow( switchToWindowAssertFunc );
			}, customPreWait, customPostWait );
		}

		/// <summary>
		/// Click OK button of an active dialog box on the page
		/// </summary>
		/// <param name="customPreWait">Custom action wait upon before clicking OK button of an active dialog box</param>
		/// <param name="customPostWait">Custom action wait upon after clicking OK button of an active dialog box</param>
		/// <returns></returns>
		public T ClickDialogBox( AssertionFunc customPreWait = null, AssertionFunc customPostWait = null )
		{
			return RunTask( b => b.ClickDialogBox(), customPreWait ?? ( a => a.DialogBoxIsActive() ), customPostWait ?? ( a => a.DialogBoxIsNotActive() ) );
		}

		/// <summary>
		/// Enter text in an active prompt dialog box on the page
		/// </summary>
		/// <param name="text">text to be entered in the prompt</param>
		/// <param name="customPreWait">Custom action wait upon before entering text in the prompt dialog box</param>
		/// <param name="customPostWait">Custom action wait upon after entering text in the prompt dialog box</param>
		/// <returns></returns>
		public T EnterTextInDialogBox( string text, AssertionFunc customPreWait = null, AssertionFunc customPostWait = null )
		{
			return RunTask( b => b.EnterTextInDialogBox( text ), customPreWait ?? ( a => a.DialogBoxIsActive() ), customPostWait );
		}

		/// <summary>
		/// Cancel an active dialog box on the page
		/// </summary>
		/// <param name="customPreWait">Custom action wait upon before cancelling the dialog box</param>
		/// <param name="customPostWait">Custom action wait upon after cancelling the dialog box</param>
		/// <returns></returns>
		public T CancelDialogBox( AssertionFunc customPreWait = null, AssertionFunc customPostWait = null )
		{
			return RunTask( b => b.CancelDialogBox(), customPreWait ?? ( a => a.DialogBoxIsActive() ), customPostWait ?? ( a => a.DialogBoxIsNotActive() ) );
		}

		/// <summary>
		/// Allow you create a custom interaction with browser
		/// </summary>
		/// <param name="task">The custom interaction with the browser</param>
		/// <param name="customPreWait">Custom action wait upon before calling custom interaction</param>
		/// <param name="customPostWait">Custom action wait upon after calling custom interaction</param>
		/// <returns></returns>
		public T Custom( Action<IXenonBrowser> task, AssertionFunc customPreWait = null, AssertionFunc customPostWait = null )
		{
			return RunTask( task, customPreWait, customPostWait );
		}
	}
}