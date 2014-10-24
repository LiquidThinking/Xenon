using System.Collections.Generic;
using Moq;

namespace Xenon.Tests
{
	public static class XenonBrowserExtensionMethods
	{
		public static void SetupFindElementsByCssSelector( this Mock<IXenonBrowser> browser, string cssSelector, Mock<IXenonElement> element, int returnElementAfterCalls = 0 )
		{
			var timesCalled = 0;
			browser.Setup( x => x.FindElementsByCssSelector( cssSelector ) )
				   .Returns( () => ++timesCalled < returnElementAfterCalls ? new List<IXenonElement>() : new List<IXenonElement>
				   {
					   element.Object
				   } );
		}		
	}
}