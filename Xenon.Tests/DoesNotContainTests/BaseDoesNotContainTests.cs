using System;
using NUnit.Framework;
using Xenon.Tests.Integration;

namespace Xenon.Tests.DoesNotContainTests
{
	public abstract class BaseDoesNotContainTests<T> : BaseXenonIntegrationTest where T : BaseXenonTest<T>
	{
		protected abstract BaseXenonTest<T> CreateInstance( IXenonBrowser browser );

		protected BaseDoesNotContainTests()
		{
			XenonTestsResourceLookup.Folder( "DoesNotContainTests" );
		}

		[Test]
		public void DoesNotContainCssSelector_WhenSelectorNotFound_AssertsTrue()
		{
			AssertDoesNotContainElement( ".missing" );
		}

		[Test]
		public void DoesNotContainCssSelector_WhenSelectorIsFound_AssertsFalse()
		{
			const string jumbo = ".jumbo";
			var assertionException = Assert.Throws<AssertionException>(
				() => AssertDoesNotContainElement( jumbo ) );

			Assert.True( assertionException.Message.Contains( $"Page contains element with selector: {jumbo}" ) );
		}

		[Test]
		public void DoesNotContainAssertionFunc_WhenNoElementsNotFound_Passing()
		{
			AssertThat( DoesNotContainHamburgers );

			XenonAssertion DoesNotContainHamburgers( XenonAssertion assertion )
			{
				return assertion.DoesNotContainElement( e => e.TextIs( "hamburgers" ) );
			}
		}

		[Test]
		public void DoesNotContainAssertionFunc_WhenElementsNotFound_Failing()
		{
			const string nothingToSeeHere = "Nothing to see here";
			var assertionException = Assert.Throws<AssertionException>(
				() => AssertThat( DoesNotContainNothingToSeeHere ) );

			AssertExceptionContains( "Page contains elements matching the following criteria", nothingToSeeHere );

			void AssertExceptionContains( params string[] values )
			{
				foreach ( var value in values )
					Assert.True( assertionException.Message.Contains( @value ) );
			}

			XenonAssertion DoesNotContainNothingToSeeHere( XenonAssertion assertion )
			{
				return assertion.DoesNotContainElement( e => e.ContainsText( nothingToSeeHere ) );
			}
		}

		private void AssertThat( AssertionFunc assertion )
			=> PerformXenonAction( x => x.Assert( assertion ) );

		private void AssertDoesNotContainElement( string cssSelector )
			=> PerformXenonAction( x => x.Assert( e => e.DoesNotContainElement( cssSelector ) ) );

		private void PerformXenonAction( Func<BaseXenonTest<T>, BaseXenonTest<T>> xenonAction )
		{
			var html = XenonTestsResourceLookup.GetContent( "DoesNotContain" );
			using ( var browserTest = new BrowserTest( html ) )
			{
				var browser = browserTest.Start();
				xenonAction( CreateInstance( browser ).GoToUrl( "/" ) );
			}
		}
	}
}