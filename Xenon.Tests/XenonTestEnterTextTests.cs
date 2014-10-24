using Moq;
using NUnit.Framework;

namespace Xenon.Tests
{
	[TestFixture]
	public class XenonTestEnterTextTests : BaseXenonTest
	{
		private const string CssSelector = "input";
		private const string ContentText = "the test content";

		[Test]
		public void EnterText_WhenCalled_EnterText()
		{
			var browser = SetupBrowser();
			var element = new Mock<IXenonElement>();
			browser.SetupFindElementsByCssSelector( CssSelector, element );

			var xenonTest = new XenonTest( browser.Object );
			xenonTest.EnterText( CssSelector, ContentText );

			element.Verify( x => x.EnterText( ContentText ) );
		}

		[Test]
		public void EnterText_WhenElementDoesNotExistStraightAway_WaitsForItThenEntersText()
		{
			var browser = SetupBrowser();
			var element = new Mock<IXenonElement>();

			browser.SetupFindElementsByCssSelector( CssSelector, element, 5 );
			var xenonTest = new XenonTest( browser.Object );
			xenonTest.EnterText( CssSelector, ContentText );

			element.Verify( x => x.EnterText( ContentText ) );
		}

		[Test]
		public void EnterText_WhenCustomPreWaitIsAssigned_ShouldWaitUponThatAssertion()
		{
			const string ready = "Ready";

			var browser = SetupBrowser();
			var element = new Mock<IXenonElement>();
			var xenonTest = new XenonTest( browser.Object );


			browser.SetupFindElementsByCssSelector( CssSelector, element );

			const int timesToCallPageSource = 5;
			var timesCalled = 0;

			browser
				.SetupGet( x => x.PageSource )
				.Returns( () => ++timesCalled < timesToCallPageSource ? string.Empty : ready );

			var calledToEarly = false;
			element.Setup( x => x.EnterText(ContentText) ).Callback( () =>
			{
				if (timesCalled != 5)
					calledToEarly = true;
			} );

			xenonTest.EnterText( CssSelector, ContentText, x => x.PageContains( ready ) );
			Assert.IsFalse( calledToEarly );
		}

		[Test]
		public void EnterText_WhenCustomPostWaitIsSet_ShouldWaitUponThatAssertion()
		{
			var browser = SetupBrowser();
			var element = new Mock<IXenonElement>();
			var xenonTest = new XenonTest( browser.Object );


			browser.SetupFindElementsByCssSelector( CssSelector, element );

			const string pageContent = "abc";

			const int timesToCallUrl = 5;
			var timesCalled = 0;

			browser
				  .SetupGet( x => x.PageSource )
				  .Returns( () => ++timesCalled < timesToCallUrl ? string.Empty : pageContent );


			xenonTest.EnterText( CssSelector, ContentText, customPostWait: a => a.PageContains( pageContent ) );

			Assert.AreEqual( timesToCallUrl, timesCalled );
		}
	}
}
