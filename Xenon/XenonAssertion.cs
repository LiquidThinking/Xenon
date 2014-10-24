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

		public XenonAssertion( IXenonBrowser xenonBrowser )
		{
			_xenonBrowser = xenonBrowser;
		}

		public bool Passing { get; private set; } = true;

		public ReadOnlyCollection<string> FailureMessages => _failureMessages.AsReadOnly();

		public XenonAssertion UrlContains( string content )
		{
			return Assert( _xenonBrowser.Url.Contains( content ), "Url does not contain: " + content );
		}

		public XenonAssertion PageContains( string content )
		{
			return Assert( _xenonBrowser.PageSource.Contains( content ), "Page does not conatain: " + content );
		}

		public XenonAssertion ContainsElement( string cssSelector )
		{
			return Assert( _xenonBrowser.FindElementsByCssSelector( cssSelector ).Any(), "Page does not conatain element with selector: " + cssSelector );
		}

		public XenonAssertion DoesNotContainElement( string cssSelector )
		{
			return Assert( !_xenonBrowser.FindElementsByCssSelector( cssSelector ).Any(), "Page conatains element with selector: " + cssSelector );
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
	}
}