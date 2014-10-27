using Moq;
using NUnit.Framework;
using Xenon.Tests.ExtensionMethods;

namespace Xenon.Tests.EnterTextTests
{
	public abstract class BaseEnterTextTests<T> : BaseXenonTest where T : BaseXenonTest<T>
	{
		private const string CssSelector = "input";
		private const string ContentText = "the test content";

		protected abstract BaseXenonTest<T> CreateInstance( Mock<IXenonBrowser> browser );


		[Test]
		public void EnterText_WhenCalled_EnterText()
		{
			var browser = SetupBrowser();
			var element = new Mock<IXenonElement>();
			browser.SetupFindElementsByCssSelector( CssSelector, element );

			var xenonTest = CreateInstance( browser );
			xenonTest.EnterText( CssSelector, ContentText );

			element.Verify( x => x.EnterText( ContentText ) );
		}

		[Test]
		public void EnterText_WhenElementDoesNotExistStraightAway_WaitsForItThenEntersText()
		{
			var browser = SetupBrowser();
			var element = new Mock<IXenonElement>();

			browser.SetupFindElementsByCssSelector( CssSelector, element, 5 );
			var xenonTest = CreateInstance( browser );
			xenonTest.EnterText( CssSelector, ContentText );

			element.Verify( x => x.EnterText( ContentText ) );
		}

		[Test]
		public void EnterText_WhenCustomPreWaitIsAssigned_ShouldWaitUponThatAssertion()
		{
			const string ready = "Ready";

			var browser = SetupBrowser();
			var element = new Mock<IXenonElement>();
			var xenonTest = CreateInstance( browser );


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
			var xenonTest = CreateInstance( browser );


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