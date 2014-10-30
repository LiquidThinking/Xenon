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

		public XenonElementsFinder LinkText( string linkTextToFind )
		{
			const string xpathFormat = "//a[contains(text(), '{0}')]";
			var criterion = string.Format( xpathFormat, linkTextToFind );
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