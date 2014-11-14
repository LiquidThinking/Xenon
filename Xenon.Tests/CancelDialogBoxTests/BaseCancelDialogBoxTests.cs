using NUnit.Framework;
using Xenon.Tests.Integration;

namespace Xenon.Tests.CancelDialogBoxTests
{
	public abstract class BaseCancelDialogBoxTests<T> : BaseXenonIntegrationTest where T : BaseXenonTest<T>
	{
		protected abstract T CreateInstance( IXenonBrowser browser );

		protected BaseCancelDialogBoxTests()
		{
			XenonTestsResourceLookup.Folder( "SharedResources" );
		}

		[Test]
		public void CancelDialogBox_DialogBoxIsActive_CancelDialogBox()
		{
			var html = XenonTestsResourceLookup.GetContent( "PageWithDialogBoxes" );
			using ( var bt = new BrowserTest( html ) )
			{
				var browser = bt.Start();
				CreateInstance( browser )
					.GoToUrl( "/" )
					.Click( where => where.TextIs( "Confirm Box" ) )
					.CancelDialogBox()
					.Assert( a => a.ContainsElement( where => where.TextIs( "Cancelled Confirm Dialog box" ) ) );
			}
		}
	}
}