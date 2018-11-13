using NUnit.Framework;
using Xenon.Tests.Integration;

namespace Xenon.Tests.MoveToElementTests
{
	public abstract class BaseMoveToElementTests<T> : BaseXenonIntegrationTest where T : BaseXenonTest<T>
	{
		protected abstract BaseXenonTest<T> CreateInstance( IXenonBrowser browser );

		public BaseMoveToElementTests()
		{
			XenonTestsResourceLookup.Folder( "MoveToElementTests" );
		}

		public void MoveToElement_WhenMovingToElement_MovesToElement()
		{
			var html = XenonTestsResourceLookup.GetContent( "MoveToElement" );
			using ( var browserTest = new BrowserTest( html ) )
			{
				var browser = browserTest.Start();

				CreateInstance( browser )
					.GoToUrl( "/" )
					.MoveToElement( "#element" );

				Assert.AreEqual( "worked", browserTest.GetPostResult()[ "temp" ] );
			}
		}
	}
}