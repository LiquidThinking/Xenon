using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
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

		private void AssertXenonActionThrowsWithMessage( PerformXenonAction<T> xenonAction, XenonElementsFinderTestCase testCase )
		{
			using ( var browserTest = new BrowserTest( XenonTestsResourceLookup.GetContent( FailureTests ) ) )
			{
				var browser = CreateInstance( browserTest.Start() );
				var exception = Assert.Throws<NoElementsFoundException>( () => xenonAction( browser, testCase ) );
				Assert.True( testCase.SearchIdentifiers.All( x => exception.Message.Contains( x ) ) );
			}
		}
	}

	delegate BaseXenonTest<T> PerformXenonAction<T>( BaseXenonTest<T> item, XenonElementsFinderTestCase testCase ) where T : BaseXenonTest<T>;

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

			const string attributeName = "not";
			const string attributeValue = "present";
			yield return new XenonElementsFinderTestCase( x => x.AttributeIs( attributeName, attributeValue ), attributeName, attributeValue );

			const string cssClass = "superStylish";
			yield return new XenonElementsFinderTestCase( x => x.CssClassIs( cssClass ), cssClass );

			yield return new XenonElementsFinderTestCase( x => x.TextIs( textIsNot ).ContainsText( textDoesNotContain ).CssClassIs( cssClass ), textIsNot, textDoesNotContain, cssClass );
		}

		public IEnumerator GetEnumerator()
		{
			return GetXenonElementsFinderTestCases().GetEnumerator();
		}
	}
}