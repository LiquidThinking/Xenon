using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Xenon.Tests.Integration;

namespace Xenon.Tests.MissingElementsTests
{
	public abstract class BaseMissingElementsTest<T> : BaseXenonIntegrationTest where T : BaseXenonTest<T>
	{
		private const string NonExistentCssSelector = ".IDontExist";
		private const string NonExistentId = "#NorMe";
		protected abstract BaseXenonTest<T> CreateInstance( IXenonBrowser browser );

		protected BaseMissingElementsTest()
		{
			XenonTestsResourceLookup.Folder( "MissingElementsTests" );
		}

		[TestCase( NonExistentCssSelector )]
		[TestCase( NonExistentId )]
		public void SearchForNonExistentElement_ByCssSelector_IncludesCssSelectorInException( string selector )
		{
			RunCssSelectorActionForExceptionMessgae( selector, ( b, css ) => b.Click( css ) );
		}


		[TestCase( NonExistentCssSelector )]
		[TestCase( NonExistentId )]
		public void ClearNonExistentElement_IncludesCssSelectorInException( string selector )
		{
			RunCssSelectorActionForExceptionMessgae( selector, ( b, css ) => b.Clear( css ) );
		}

		[TestCase( NonExistentCssSelector )]
		[TestCase( NonExistentId )]
		public void EnterTextIntoNonExistentElement_IncludesCssSelectorInException( string selector )
		{
			RunCssSelectorActionForExceptionMessgae( selector, ( b, css ) => b.EnterText( css, string.Empty ) );
		}

		private void RunCssSelectorActionForExceptionMessgae( string selector, Func<BaseXenonTest<T>, string, BaseXenonTest<T>> xenonAction )
		{
			using ( var browserTest = CreateBrowserTest() )
			{
				var browser = CreateInstance( browserTest.Start() );
				var exception = Assert.Throws<NoElementsFoundException>( () => xenonAction( browser, selector ) );
				Assert.True( exception.Message.Contains( selector ) );
			}
		}

		[TestCaseSource( typeof( XenonElementsFinderTestCase ) )]
		public void ClickNonExistentElement_IncludesTextInException( XenonElementsFinderTestCase testCase )
		{
			AssertXenonActionThrowsWithMessage( ( x, tc ) => x.Click( tc.SearchCriteria ), testCase );
		}

		[TestCaseSource( typeof( XenonElementsFinderTestCase ) )]
		public void RightClickNonExistentElement_IncludesTextInException( XenonElementsFinderTestCase testCase )
		{
			AssertXenonActionThrowsWithMessage( ( x, tc ) => x.RightClick( tc.SearchCriteria ), testCase );
		}

		[Test]
		public void CustomForNonExistentElement_IncludesTextInException()
		{
			const string cssSelector = "not there";
			using ( var browserTest = CreateBrowserTest() )
			{
				var browser = CreateInstance( browserTest.Start() );
				var exception = Assert.Throws<NoElementsFoundException>( () => browser.Custom( x => x.FindElementsByCssSelector( cssSelector ) ) );
				Assert.True( exception.Message.Contains( cssSelector ) );
			}
		}

		private void AssertXenonActionThrowsWithMessage( 
			Func<BaseXenonTest<T>, XenonElementsFinderTestCase, BaseXenonTest<T>> xenonAction, 
			XenonElementsFinderTestCase testCase )
		{
			using ( var browserTest = CreateBrowserTest() )
			{
				var browser = CreateInstance( browserTest.Start() );
				var exception = Assert.Throws<NoElementsFoundException>( () => xenonAction( browser, testCase ) );
				Assert.True( testCase.SearchIdentifiers.All( x => exception.Message.Contains( x ) ) );
			}
		}

		private BrowserTest CreateBrowserTest() => new BrowserTest( "Empty" );
	}

	public class XenonElementsFinderTestCase : IEnumerable
	{
		public List<string> SearchIdentifiers { get; }
		public Func<XenonElementsFinder, XenonElementsFinder> SearchCriteria { get; }

		public XenonElementsFinderTestCase( Func<XenonElementsFinder, XenonElementsFinder> searchCriteria, params string[] searchIdentifiers )
		{
			SearchIdentifiers = searchIdentifiers.ToList();
			SearchCriteria = searchCriteria;
		}

		//do not remove, needed by NUnit & must be public
		public XenonElementsFinderTestCase() { }

		private static IEnumerable<XenonElementsFinderTestCase> GetXenonElementsFinderTestCases()
		{
			const string textIsNot = nameof( textIsNot );
			yield return new XenonElementsFinderTestCase( x => x.TextIs( textIsNot ), textIsNot );

			const string textDoesNotContain = nameof( textDoesNotContain );
			yield return new XenonElementsFinderTestCase( x => x.ContainsText( textDoesNotContain ), textDoesNotContain );

			const string attributeName = nameof( attributeName );
			const string attributeValue = nameof( attributeValue );
			yield return new XenonElementsFinderTestCase( x => x.AttributeIs( attributeName, attributeValue ), attributeName, attributeValue );

			const string cssClass = nameof( cssClass );
			yield return new XenonElementsFinderTestCase( x => x.CssClassIs( cssClass ), cssClass );

			yield return new XenonElementsFinderTestCase( x => x.TextIs( textIsNot ).ContainsText( textDoesNotContain ).CssClassIs( cssClass ), textIsNot, textDoesNotContain, cssClass );
		}

		public IEnumerator GetEnumerator()
		{
			return GetXenonElementsFinderTestCases().GetEnumerator();
		}
	}
}