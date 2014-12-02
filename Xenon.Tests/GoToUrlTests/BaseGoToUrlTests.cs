using Moq;
using NUnit.Framework;

namespace Xenon.Tests.GoToUrlTests
{
	public abstract class BaseGoToUrlTests<T> : BaseXenonTest where T : BaseXenonTest<T>
	{
		private const string Url = "/A/Test/Url";

		private class SetupGoToUrlResult
		{
			public Mock<IXenonBrowser> Browser { get; set; }
			public BaseXenonTest<T> XenonTest { get; set; }
		}


		private SetupGoToUrlResult SetupGoToUrl()
		{
			var browser = SetupBrowser();
			var result = new SetupGoToUrlResult
			{
				Browser = browser,
				XenonTest = CreateInstance( browser )
			};

			return result;
		}

		protected abstract BaseXenonTest<T> CreateInstance( Mock<IXenonBrowser> browser );

		[Test]
		public void GoToUrl_WhenPageIsLoadedStraightAway_CallsBrowserGoToUrl()
		{
			var result = SetupGoToUrl();

			result.XenonTest.GoToUrl( Url, customPostWait: EmptyAssertion );
			result.Browser.Verify( x => x.GoToUrl( Url ) );
		}

		[Test]
		public void GoToUrl_WhenPageTakesTimeToLoad_ShouldWaitAndThenReturn()
		{
			var result = SetupGoToUrl();

			const int timesToCallUrl = 5;
			var timesCalled = 0;

			result.Browser.SetupGet( x => x.Url )
			      .Returns( () => ++timesCalled < timesToCallUrl ? string.Empty : Url );

			result.XenonTest.GoToUrl( Url );

			Assert.AreEqual( 6, timesCalled );
		}

		[Test]
		public void GoToUrl_WhenCustomPostWaitIsSet_ShouldWaitUponThatAssertion()
		{
			var result = SetupGoToUrl();
			const string pageContent = "abc";

			const int timesToCallUrl = 5;
			var timesCalled = 0;

			result.Browser
			      .SetupGet( x => x.PageSource )
			      .Returns( () => ++timesCalled < timesToCallUrl ? string.Empty : pageContent );


			result.XenonTest.GoToUrl( Url, customPostWait: a => a.PageContains( pageContent ) );

			Assert.AreEqual( timesToCallUrl, timesCalled );
		}

		[Test]
		public void GoToUrl_WhenCustomPreWaitIsSet_ShouldWaitUponThatAssertion()
		{
			var result = SetupGoToUrl();

			const string pageContent = "abc";

			const int timesToCallPageSource = 5;
			var timesCalled = 0;
			result.Browser
			      .SetupGet( x => x.PageSource )
			      .Returns( () => ++timesCalled < timesToCallPageSource ? string.Empty : pageContent );

			var calledToEarly = false;
			result.Browser.Setup( x => x.GoToUrl( It.IsAny<string>() ) ).Callback( () =>
			{
				if ( timesCalled != 5 )
					calledToEarly = true;
			} );


			result.XenonTest.GoToUrl( Url, a => a.PageContains( pageContent ), EmptyAssertion );

			Assert.IsFalse( calledToEarly );
		}
	}
}