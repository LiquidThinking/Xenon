using NUnit.Framework;
using Xenon.Selenium;
using Xenon.Tests.Integration;

namespace Xenon.Tests.FailureTests
{
	public abstract class BaseFailureTests<T> : BaseXenonIntegrationTest where T : BaseXenonTest<T>
	{
		private const string FailureTests = "FailureTests";
		protected abstract BaseXenonTest<T> CreateInstance( IXenonBrowser browser );

		protected BaseFailureTests()
		{
			XenonTestsResourceLookup.Folder( FailureTests );
		}

		[TestCase( "#IDontExist" )]
		[TestCase( ".NorMe" )]
		public void SearchForNonExistentElement_ByCssSelector_IncludesCssSelectorInException( string selector )
		{
			using ( var browserTest = new BrowserTest( XenonTestsResourceLookup.GetContent( FailureTests ) ) )
			{
				var exception = Assert.Throws<NoElementsFoundException>( () => CreateInstance( browserTest.Start() )
																			.Click( selector ) );
				Assert.True( exception.Message.Contains( $"No elements found with selector '{selector}'" ) );
			}
		}

		[Test]
		public void SearchForNonExistentElement_ByTextIs_IncludesTextInException()
		{
			const string text = "Hello";
			using ( var browserTest = new BrowserTest( XenonTestsResourceLookup.GetContent( FailureTests ) ) )
			{
				var exception = Assert.Throws<NoElementsFoundException>( () => CreateInstance( browserTest.Start() )
																			.Click( x => x.TextIs( text ) ) );
				Assert.True( exception.Message.Contains( text ) );
			}
		}
	}
}