using System;
using System.Collections.Generic;
using System.Linq;

namespace Xenon
{

	public class XenonElementsFinder
	{
		private readonly IXenonBrowser _browser;
		private readonly List<string> _xpathCriteria;

		public XenonElementsFinder( IXenonBrowser browser )
		{
			_browser = browser;
			_xpathCriteria = new List<string>();
		}

		public XenonElementsFinder TextIs( string text )
		{
			const string xpathFormat = "(//input[@value='{0}' and (@type='submit' or @type='button' or @type= 'reset' ) ] | //*[text() = '{0}'])[position()=1]";
			var criterion = string.Format( xpathFormat, text );
			_xpathCriteria.Add( criterion );

			return this;
		}

		internal IEnumerable<IXenonElement> FindElements( )
		{
			var criteria = String.Join( " and ", _xpathCriteria );
			var result = _browser.FindElementsByXPath( criteria );

			return result;
		}
	}
}