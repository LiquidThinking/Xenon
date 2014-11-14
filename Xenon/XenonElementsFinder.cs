using System;
using System.Collections.Generic;
using System.Linq;

namespace Xenon
{
	public class XenonElementsFinder
	{
		private struct XpathCriteria
		{
			public string XPathExpression { get; private set; }
			private bool SearchInAllNodes { get; set; }

			public XpathCriteria( string xPathExpression, bool searchInAllNodes ) : this()
			{
				XPathExpression = xPathExpression;
				SearchInAllNodes = searchInAllNodes;
			}

			public string GetSearchInAllNodesCriteria()
			{
				if ( SearchInAllNodes )
					return XPathExpression;
				else
					return String.Concat( "//*", XPathExpression );
			}
		}

		private readonly IXenonBrowser _browser;
		private readonly List<XpathCriteria> _xpathCriteria;

		public XenonElementsFinder( IXenonBrowser browser )
		{
			_browser = browser;
			_xpathCriteria = new List<XpathCriteria>();
		}

		public XenonElementsFinder TextIs( string text )
		{
			const string xpathFormat = "(//input[@value='{0}' and (@type='submit' or @type='button' or @type= 'reset' ) ] | //*[text() = '{0}'])";
			var criterion = string.Format( xpathFormat, text );
			_xpathCriteria.Add( new XpathCriteria( criterion, true ) );

			return this;
		}

		public XenonElementsFinder AttributeIs( string attributeName, string attributeValue )
		{
			const string xpathFormat = "[@{0}='{1}']";

			var criterion = string.Format( xpathFormat, attributeName, attributeValue );
			_xpathCriteria.Add( new XpathCriteria( criterion, false ) );

			return this;
		}


		internal IEnumerable<IXenonElement> FindElements()
		{
			string criteria;

			if ( _xpathCriteria.Count == 1 )
				criteria = _xpathCriteria.First().GetSearchInAllNodesCriteria();
			else
				criteria = String.Join( "", _xpathCriteria.Select( x => x.XPathExpression ) );


			var result = _browser.FindElementsByXPath( criteria );

			return result;
		}
	}
}