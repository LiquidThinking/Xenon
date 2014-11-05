using System;
using System.Collections.Generic;

namespace Xenon
{
	public interface IXenonBrowser : IDisposable
	{
		string Url { get; }
		string PageSource { get; }
		IEnumerable<IXenonElement> FindElementsByCssSelector( string cssSelector );
		void GoToUrl( string url );
		XenonAssertion RunAssertion( AssertionFunc assertion );
		void Quit();
	}
}