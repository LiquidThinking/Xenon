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

		public ReadOnlyCollection<string> FailureMessages
		{
			get { return _failureMessages.AsReadOnly(); }
		}

		public XenonAssertion UrlContains( string content )
		{
			return Assert( _xenonBrowser.Url.Contains( content ), String.Format( "Url '{0}' does not contain: {1}", _xenonBrowser.Url, content ) );
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
			return Assert( where( new XenonElementsFinder( _xenonBrowser ) ).FindElements().Any( e => e.IsVisible ), "Page does not contain element with selector: " );
		}

		public XenonAssertion DoesNotContainElement( string cssSelector )
		{
			return Assert( !_xenonBrowser.FindElementsByCssSelector( cssSelector ).Any( e => e.IsVisible ), "Page contains element with selector: " + cssSelector );
		}

		public XenonAssertion DoesNotContainElement( Func<XenonElementsFinder, XenonElementsFinder> where )
		{
			return Assert( !where( new XenonElementsFinder( _xenonBrowser ) ).FindElements().Any( e => e.IsVisible ), "Page does contains element with selector: " );
		}

		public XenonAssertion CustomAssertion( Func<IXenonBrowser, bool> customFunc )
		{
			return Assert( customFunc( _xenonBrowser ), "Custom assertion failed" );
		}

		private XenonAssertion Assert( bool contains, string message )
		{
			if ( !contains )
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