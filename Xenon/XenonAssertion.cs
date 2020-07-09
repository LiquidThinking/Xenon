using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Xenon
{
	public class XenonAssertion
	{
		private readonly List<string> _failureMessages = new List<string>();
		private readonly IXenonBrowser _xenonBrowser;

		public bool Passing { get; private set; }

		public XenonAssertion( IXenonBrowser xenonBrowser )
		{
			_xenonBrowser = xenonBrowser;
			Passing = true;
		}

		public ReadOnlyCollection<string> FailureMessages => _failureMessages.AsReadOnly();

		public XenonAssertion UrlContains( string content )
		{
			return Assert( _xenonBrowser.Url.Contains( content ), $"Url '{_xenonBrowser.Url}' does not contain: {content}" );
		}

		public XenonAssertion PageContains( string content )
		{
			var pageContains = _xenonBrowser.PageSource.Contains( content );
			if ( !pageContains )
				pageContains = _xenonBrowser.FindElementsByCssSelector( "input, textarea" ).Any( e => e.Text.Contains( content ) );

			return Assert( pageContains, "Page does not contain: " + content );
		}

		public XenonAssertion PageDoesNotContain( string text )
		{
			return Assert( !_xenonBrowser.PageSource.Contains( text ), "Page contains: " + text );
		}

		public XenonAssertion ContainsElement( string cssSelector )
		{
			return Assert( _xenonBrowser.FindElementsByCssSelector( cssSelector ).Any( e => e.IsVisible ), "Page does not contain element with selector: " + cssSelector );
		}

		public XenonAssertion ContainsElement( Func<XenonElementsFinder, XenonElementsFinder> where )
		{
			return BrowserContainsElement( where, shouldContainElement: true );
		}

		public XenonAssertion DoesNotContainElement( string cssSelector )
		{
			var searchResult = _xenonBrowser.FindElementsByCssSelector( cssSelector );
			return Assert( !searchResult.Any( e => e.IsVisible ), "Page contains element with selector: " + cssSelector );
		}

		public XenonAssertion DoesNotContainElement( Func<XenonElementsFinder, XenonElementsFinder> where )
		{
			return BrowserContainsElement( where, shouldContainElement: false );
		}

		private XenonAssertion BrowserContainsElement( Func<XenonElementsFinder, XenonElementsFinder> where, bool shouldContainElement )
		{
			var searchResult = where( new XenonElementsFinder( _xenonBrowser ) ).FindElements();
			var elementsArePresentAndVisible = searchResult.Any( e => e.IsVisible );
			return Assert(
				elementsArePresentAndVisible == shouldContainElement,
				$"Page contains elements matching the following criteria: {searchResult}" );
		}

		public XenonAssertion CustomAssertion( Func<IXenonBrowser, string> customFunc )
		{
			var errorMessage = customFunc( _xenonBrowser );
			return Assert( 
				passing: string.IsNullOrEmpty( errorMessage ),
				message: errorMessage );
		}

		public XenonAssertion CustomAssertion( Func<IXenonBrowser, bool> customFunc, string errorMessage = null )
		{
			return Assert( customFunc( _xenonBrowser ), errorMessage ?? "Custom assertion failed" );
		}

		private XenonAssertion Assert( bool passing, string message )
		{
			if ( !passing )
			{
				Passing = false;
				_failureMessages.Add( message );
			}

			return this;
		}

		internal XenonAssertion BrowserFailure( string message )
		{
			Passing = false;
			_failureMessages.Add( message );

			return this;
		}

		public XenonAssertion DialogBoxIsActive()
		{
			return Assert( _xenonBrowser.DialogBoxIsActive(), "There is no active dialog box on the page" );
		}

		public XenonAssertion DialogBoxIsNotActive()
		{
			return Assert( !_xenonBrowser.DialogBoxIsActive(), "A dialog box is still active on the page" );
		}
	}
}