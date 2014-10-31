using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Xenon.Tests.ExtensionMethods;

namespace Xenon.Tests.ClickTests
{
	public abstract class BaseClickTests<T> : BaseXenonTest where T : BaseXenonTest<T>
	{
		protected abstract BaseXenonTest<T> CreateInstance( Mock<IXenonBrowser> browser );

		[Test]
		public void Click_WhenClicked_CallsBrowserClick()
		{
			const string cssSelector = "button";
			var element = new Mock<IXenonElement>();

			var browser = SetupBrowser();
			browser.SetupFindElementsByCssSelector( cssSelector, element );

			var xenonTest = CreateInstance( browser );
			xenonTest.Click( cssSelector );

			element.Verify( x => x.Click() );
		}

		[Test]
		public void Click_WhenElementDoesNotExistStraightAway_WaitsForItThenClicks()
		{
			const string cssSelector = "button";
			var element = new Mock<IXenonElement>();

			var browser = SetupBrowser();
			browser.SetupFindElementsByCssSelector( cssSelector, element, 5 );

			var xenonTest = CreateInstance( browser );
			xenonTest.Click( cssSelector );

			element.Verify( x => x.Click() );
		}

		[Test]
		public void Click_WhenCustomPreWaitIsAssigned_ShouldWaitUponThatAssertion()
		{
			const string cssSelector = "button";
			const string content = "Ready";
			var element = new Mock<IXenonElement>();

			const int timesToCallUrl = 5;
			var timesCalled = 0;

			var browser = SetupBrowser();
			browser.SetupGet( x => x.PageSource )
			       .Returns( () => ++timesCalled < timesToCallUrl ? string.Empty : content );

			browser.SetupFindElementsByCssSelector( cssSelector, element );
			var calledToEarly = false;
			element.Setup( x => x.Click() ).Callback( () =>
			{
				if ( timesCalled != 5 )
					calledToEarly = true;
			} );

			var xenonTest = CreateInstance( browser );
			xenonTest.Click( cssSelector, x => x.PageContains( content ) );

			Assert.IsFalse( calledToEarly );
		}

		[Test]
		public void Click_WhenCustomPostWaitIsAssigned_ShouldWaitUponThatAssertion()
		{
			const string cssSelector = "button";
			const string content = "Ready";
			var element = new Mock<IXenonElement>();

			const int timesToCallUrl = 5;
			var timesCalled = 0;

			var browser = SetupBrowser();
			browser.SetupGet( x => x.PageSource )
			       .Returns( () => ++timesCalled < timesToCallUrl ? string.Empty : content );

			browser.Setup( x => x.FindElementsByCssSelector( cssSelector ) )
			       .Returns( new List<IXenonElement>
			       {
				       element.Object
			       } );

			var xenonTest = CreateInstance( browser );
			xenonTest.Click( cssSelector, EmptyAssertion, x => x.PageContains( content ) );


			Assert.AreEqual( timesToCallUrl, timesCalled );
		}

		[Test]
		public void Click_WhereTextIsProvided_ShouldCallTheClickOnElementFound()
		{
			const string linkText = "Click Me";


			var mockedElement = new Mock<IXenonElement>();
			var browser = SetupBrowser();
			browser
				.Setup( x => x.FindElementsByXPath( It.IsAny<string>() ) )
				.Returns( new List<IXenonElement>
				{
					mockedElement.Object
				} );

			var xenonTest = CreateInstance( browser );
			xenonTest.Click( x => x.TextIs( linkText ) );

			mockedElement.Verify( x => x.Click() );
		}
	}
}