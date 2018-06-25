using System.Collections.Generic;
using System.Linq;
using Moq;

namespace Xenon.Tests.ExtensionMethods
{
	public static class XenonBrowserExtensionMethods
	{
		public static void SetupFindElementsByCssSelector( this Mock<IXenonBrowser> browser, string cssSelector, Mock<IXenonElement> element, int returnElementAfterCalls = 0 )
		{
			var timesCalled = 0;
			browser.Setup( x => x.FindElementsByCssSelector( cssSelector ) )
				   .Returns( () =>
				   {
					   if ( ++timesCalled >= returnElementAfterCalls )
						   element.SetupGet( x => x.IsVisible ).Returns( true );
					   return new XenonElementsSearchResult( new List<IXenonElement>
					   {
						   element.Object
					   }, "Searching for css selector" );
				   } );
		}

		public static void SetupFindElementsByXPath( this Mock<IXenonBrowser> browser, string xpath, params Mock<IXenonElement>[] elements  )
		{
			browser
				.Setup( x => x.FindElementsByXPath( It.IsAny<string>() ) )
				.Returns( new XenonElementsSearchResult( elements.Select( x => x.Object ).ToList(), "Seaching for XPath" ) );

		}
	}
}