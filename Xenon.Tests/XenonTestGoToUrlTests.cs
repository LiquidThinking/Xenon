using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

namespace Xenon.Tests
{
	[TestFixture]
	public class XenonTestGoToUrlTests
	{
		private const string Url = "/A/Test/Url";

		private class SetupGoToUrlResult
		{
			public Mock<IXenonBrowser> Browser { get; set; }
			public XenonTest XenonTest { get; set; }
			public int TimesUrlCalled { get; set; }
		}

		private SetupGoToUrlResult SetupGoToUrl( string url, int timesToCallUrl = 0 )
		{
			var browser = new Mock<IXenonBrowser>();

			var result = new SetupGoToUrlResult
			{
				Browser = browser,
				XenonTest = new XenonTest( browser.Object )
			};

			browser.SetupGet( x => x.Url )
			       .Returns( () => ++result.TimesUrlCalled < timesToCallUrl ? string.Empty : url );

			browser.Setup( x => x.RunAssertion( It.IsAny<Func<XenonAssertion, XenonAssertion>>() ) )
			       .Returns<Func<XenonAssertion, XenonAssertion>>( x => x( new XenonAssertion( browser.Object ) ) );

			return result;
		}

		[Test]
		public void GoToUrl_WhenPageIsLoadedStraightAway_GoesToUrlAndReturnsInstantly()
		{
			var result = SetupGoToUrl( Url );

			result.XenonTest.GoToUrl( Url );

			result.Browser.Verify( x => x.GoToUrl( Url ) );
		}

		[Test]
		public void GoToUrl_WhenPageTakesTimeToLoad_ShouldWaitAndThenReturn()
		{
			var result = SetupGoToUrl( Url, 5 );

			result.XenonTest.GoToUrl( Url );
			Assert.AreEqual( 5, result.TimesUrlCalled );
		}

		[Test]
		public void GoToUrl_WhenCustomPostWaitIsSet_ShouldWaitUponThatAssertion()
		{
			var result = SetupGoToUrl( Url );
			const string pageContent = "abc";

			const int timesToCallUrl = 5;
			int timesCalled = 0;

			result.Browser
			      .SetupGet( x => x.PageSource )
			      .Returns( () => ++timesCalled < timesToCallUrl ? string.Empty : pageContent );


			result.XenonTest.GoToUrl( Url, a => a.PageContains( pageContent ) );

			Assert.AreEqual( timesToCallUrl, timesCalled );
		}
	}
}