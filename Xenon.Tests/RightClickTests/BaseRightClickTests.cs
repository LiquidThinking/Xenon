using System;
using System.Collections.Generic;
using Moq;
using Moq.Language.Flow;
using NUnit.Framework;
using Xenon.Tests.ExtensionMethods;

namespace Xenon.Tests.RightClickTests
{
	public abstract class BaseRightClickTests<T> : BaseXenonTest where T : BaseXenonTest<T>
	{
		protected abstract BaseXenonTest<T> CreateInstance( Mock<IXenonBrowser> browser );

		[Test]
		public void RightClick_WhenRightClicked_CallsBrowserRightClick()
		{
			const string cssSelector = "button";
			var element = new Mock<IXenonElement>();
		    element.Setup( x => x.ScrollToElement() ).Returns( () => element.Object );

			var browser = SetupBrowser();
			browser.SetupFindElementsByCssSelector( cssSelector, element );

			SetupExpectedSequenceForElement( element ).Returns( () => element.Object );

			var xenonTest = CreateInstance( browser );
			xenonTest.RightClick( cssSelector );

			element.VerifyAll();
		}

		private static ISetup<IXenonElement, IXenonElement> SetupExpectedSequenceForElement( Mock<IXenonElement> element )
		{
			var sequence = new MockSequence();
			element.InSequence( sequence ).Setup( x => x.ScrollToElement() ).Returns( () => element.Object );
			return element.InSequence( sequence ).Setup( x => x.RightClick() );
		}

		[Test]
		public void RightClick_WhenElementDoesNotExistStraightAway_WaitsForItThenRightClicks()
		{
			const string cssSelector = "button";
			var element = new Mock<IXenonElement>();
            element.Setup( x => x.ScrollToElement() ).Returns( () => element.Object );

            var browser = SetupBrowser();
			browser.SetupFindElementsByCssSelector( cssSelector, element, 5 );

			SetupExpectedSequenceForElement( element ).Returns( () => element.Object );

			var xenonTest = CreateInstance( browser );
			xenonTest.RightClick( cssSelector );

			element.VerifyAll();
		}

		[Test]
		public void RightClick_WhenCustomPreWaitIsAssigned_ShouldWaitUponThatAssertion()
		{
			const string cssSelector = "button";
			const string content = "Ready";
			var element = new Mock<IXenonElement>();
            element.Setup( x => x.ScrollToElement() ).Returns( () => element.Object );

            const int timesToCallUrl = 5;
			var timesCalled = 0;

			var browser = SetupBrowser();
			browser.SetupGet( x => x.PageSource )
			       .Returns( () => ++timesCalled < timesToCallUrl ? string.Empty : content );

			browser.SetupFindElementsByCssSelector( cssSelector, element );
			var calledToEarly = false;

			SetupExpectedSequenceForElement( element ).Callback( () =>
			{
				if ( timesCalled != 5 )
					calledToEarly = true;
			} );

			var xenonTest = CreateInstance( browser );
			xenonTest.RightClick( cssSelector, x => x.PageContains( content ) );

			Assert.IsFalse( calledToEarly );
		}

		[Test]
		public void RightClick_WhenCustomPostWaitIsAssigned_ShouldWaitUponThatAssertion()
		{
			const string cssSelector = "button";
			const string content = "Ready";
			var element = new Mock<IXenonElement>();
            element.Setup( x => x.ScrollToElement() ).Returns( () => element.Object );
            element.Setup( x => x.IsVisible ).Returns( true );

			const int timesToCallUrl = 5;
			var timesCalled = 0;

			var browser = SetupBrowser();
			browser.SetupGet( x => x.PageSource )
			       .Returns( () => ++timesCalled < timesToCallUrl ? string.Empty : content );

			browser.Setup( x => x.FindElementsByCssSelector( cssSelector ) )
				.Returns( new XenonElementSearchResult( new List<IXenonElement>
				{
					element.Object
				}, "Searching for css selector" ) );

			SetupExpectedSequenceForElement( element );

			var xenonTest = CreateInstance( browser );
			xenonTest.RightClick( cssSelector, EmptyAssertion, x => x.PageContains( content ) );


			Assert.AreEqual( timesToCallUrl, timesCalled );
		}
	}
}