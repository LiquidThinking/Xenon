using System;
using Moq;
using NUnit.Framework;

namespace Xenon.Tests.AssertTests
{
	public abstract class BaseAssertTests<T> : BaseXenonTest where T : BaseXenonTest<T>
	{
		[Test]
		public void Assert_WhenAssertionPasses_CallsAssertMethodWithTrue()
		{
			var assertMethodCalled = false;

			var browser = SetupBrowser();
			var xenonTestOptions = new XenonTestOptions
			{
				AssertMethod = ( a, b ) => assertMethodCalled = a
			};
			var xenonTest = CreateInstance( browser, xenonTestOptions );

			xenonTest.Assert( a => a.CustomAssertion( b => true ) );

			Assert.IsTrue( assertMethodCalled );
		}

		protected abstract BaseXenonTest<T> CreateInstance( Mock<IXenonBrowser> browser, XenonTestOptions xenonTestOptions );

		[Test]
		public void Assert_WhenAssertionsFails_CallsAssertMethodWithFalse()
		{
			var assertMethodCalled = false;

			var browser = SetupBrowser();
			var xenonTest = CreateInstance( browser, new XenonTestOptions
			{
				AssertMethod = ( a, b ) => assertMethodCalled = a == false
			} );

			xenonTest.Assert( a => a.CustomAssertion( b => false ) );

			Assert.IsTrue( assertMethodCalled );
		}

		[Test]
		public void Assert_WhenAssertionDoesNotPassStraightAway_CallsAssertMethodWithTrue()
		{
			var assertMethodCalled = false;
			var browser = SetupBrowser();
			var xenonTest = CreateInstance( browser, new XenonTestOptions
			{
				AssertMethod = ( a, b ) => assertMethodCalled = a
			} );

			var timesCalled = 0;
			xenonTest.Assert( a => a.CustomAssertion( b => ++timesCalled >= 5 ) );

			Assert.IsTrue( assertMethodCalled );
		}

		private XenonTestOptions GetXenonTestOptions( Action<bool, string> assertMethod = null )
		{
			return new XenonTestOptions
			{
				AssertMethod = assertMethod ?? Assert.IsTrue
			};
		}

		[Test]
		public void Assert_PageDoesNotContains_ReturnsTrueWhenContentNotFound()
		{
			const string pageSource = "A tiny line used in page source property for testing";
			var browser = SetupBrowser();
			browser.SetupGet( x => x.PageSource ).Returns( pageSource );

			var tester = CreateInstance( browser, GetXenonTestOptions() );

			tester.Assert( x => x.PageDoesNotContains( "Error" ) );
		}

		[Test]
		public void Assert_PageDoesNotContains_ReturnsFalseWhenContentFound()
		{
			const string pageSource = "A tiny line for Error used in page source property for testing";
			var browser = SetupBrowser();
			browser.SetupGet( x => x.PageSource ).Returns( pageSource );

			var tester = CreateInstance( browser, GetXenonTestOptions( Assert.IsFalse ) );

			tester.Assert( x => x.PageDoesNotContains( "Error" ) );
		}
	}
}