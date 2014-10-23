using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xenon
{
	public interface IXenonBrowser
	{
		string Url { get; }
		string PageSource { get; }
		IEnumerable<IXenonElement> FindElementsByCssSelector( string cssSelector );
		void GoToUrl( string url );
		XenonAssertion RunAssertion( Func<XenonAssertion, XenonAssertion> assertion );
	}
}
