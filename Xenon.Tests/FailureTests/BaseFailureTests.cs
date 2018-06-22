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
		public void ClickNonExistentElement_ByTextIs_IncludesTextInException( XenonElementsFinderTestCase testCase )
		{
			using ( var browserTest = new BrowserTest( XenonTestsResourceLookup.GetContent( FailureTests ) ) )
			{
				var exception = Assert.Throws<NoElementsFoundException>( () => CreateInstance( browserTest.Start() )
																			.Click( testCase.SearchCriteria ) );
				Assert.True( testCase.SearchIdentifiers.All( x => exception.Message.Contains( x ) ) );
			}
		}		
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

		public XenonElementsFinderTestCase()
		{
		}

		private static IEnumerable<XenonElementsFinderTestCase> GetXenonElementsFinderTestCases()
		{
			const string textIsNot = nameof( textIsNot );
			yield return new XenonElementsFinderTestCase( x => x.TextIs( textIsNot ), textIsNot );
		}

		public IEnumerator GetEnumerator()
		{
			return GetXenonElementsFinderTestCases().GetEnumerator();
		}
	}
}