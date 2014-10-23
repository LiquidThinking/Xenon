using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Xenon
{
	public class XenonTest
	{
		private readonly IXenonBrowser _xenonBrowser;

		public XenonTest( IXenonBrowser xenonBrowser )
		{
			_xenonBrowser = xenonBrowser;
		}

		private void WaitUntil( Func<XenonAssertion, XenonAssertion> wait )
		{
			var endTime = DateTime.Now.AddSeconds( 5 );

			do
			{
				if ( _xenonBrowser.RunAssertion( wait ).Passing )
					break;

				Thread.Sleep( 10 );
			} while ( DateTime.Now < endTime );
		}

		private XenonTest RunTask( Action<IXenonBrowser> task, Func<XenonAssertion, XenonAssertion> postWait = null )
		{
			task( _xenonBrowser );

			if ( postWait != null )
				WaitUntil( postWait );

			return this;
		}

		public XenonTest GoToUrl( string url, Func<XenonAssertion, XenonAssertion> customPostWait = null )
		{
			return RunTask( w => w.GoToUrl( url ), postWait: customPostWait ?? ( a => a.UrlContains( url ) ) );
		}
	}
}