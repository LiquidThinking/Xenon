using NUnit.Framework;

namespace Xenon.Tests
{
	public class XenonTestAssertTests : BaseXenonTest
	{
		[Test]
		public void Assert_WhenAssertionPasses_CallsAssertMethodWithTrue()
		{
			var assertMethodCalled = false;

			var xenonTest = new XenonTest( null, new XenonTestOptions
			{
				AssertMethod = ( a, b ) => assertMethodCalled = a
			} );

			xenonTest.Assert( a => a.CustomAssertion( b => true ) );

			Assert.IsTrue( assertMethodCalled );
		}

		[Test]
		public void Assert_WhenAssertionsFails_CallsAssertMethodWithFalse()
		{
			var assertMethodCalled = false;

			var xenonTest = new XenonTest( null, new XenonTestOptions
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
			var xenonTest = new XenonTest( browser.Object, new XenonTestOptions
			{
				AssertMethod = ( a, b ) => assertMethodCalled = a
			} );

			var timesCalled = 0;
			xenonTest.Assert( a => a.CustomAssertion( b => ++timesCalled >= 5 ) );

			Assert.IsTrue( assertMethodCalled );
		}
	}
}