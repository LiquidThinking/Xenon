using System;
using System.Collections.Generic;

namespace Xenon
{
	public interface IXenonBrowser : IDisposable
	{
		string Url { get; }
		string PageSource { get; }
		IEnumerable<IXenonElement> FindElementsByCssSelector( string cssSelector );
		IEnumerable<IXenonElement> FindElementsByXPath( string xpath );
		void GoToUrl( string url );
		XenonAssertion RunAssertion( AssertionFunc assertion );
		void Quit();
		IXenonBrowser SwitchToWindow( AssertionFunc assertion );
		void CloseWindow();
		IXenonBrowser ClickDialogBox();
		bool DialogBoxIsActive();
		IXenonBrowser EnterTextInDialogBox( string text );
		IXenonBrowser CancelDialogBox();
		void TakeScreenshot( string path );
		void ClearLocalStorage();
		void ClearSessionStorage();
		void ClearCookies();
	}
}