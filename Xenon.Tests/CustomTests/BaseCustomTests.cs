using Moq;
using NUnit.Framework;

namespace Xenon.Tests.CustomTests
{
	public abstract class BaseCustomTests<T> : BaseXenonTest where T : BaseXenonTest<T>
	{
		protected abstract T CreateInstance( IXenonBrowser browser );

		[Test]
		public void Custom_RunsCustomBrowserInteraction()
		{
			const string interactionCalled = "Interaction Called";

			var browser = new Mock<IXenonBrowser>();
			var test = CreateInstance( browser.Object );

			test.Custom( b => b.GoToUrl( interactionCalled ) );

			browser.Verify( b => b.GoToUrl( interactionCalled ) );
		}

		[Test]
		public void Custom_CallsPreWait()
		{
			const string interactionCalled = "Interaction Called";

			var browser = SetupBrowser();
			var test = CreateInstance( browser.Object );

			var preWaitCalled = false;

			test.Custom( b => b.GoToUrl( interactionCalled ), a=>a.CustomAssertion( b => preWaitCalled = true ) );

			Assert.IsTrue( preWaitCalled );
		}

		[Test]
		public void Custom_CallsPostWait()
		{
			const string interactionCalled = "Interaction Called";

			var browser = SetupBrowser();
			var test = CreateInstance( browser.Object );

			var postWaitCalled = false;

			test.Custom( b => b.GoToUrl( interactionCalled ),customPostWait: a => a.CustomAssertion( b => postWaitCalled = true ) );

			Assert.IsTrue( postWaitCalled );
		}
	}
}