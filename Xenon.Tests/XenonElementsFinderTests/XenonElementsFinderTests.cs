using System;
using NUnit.Framework;
using Xenon.Tests.Integration;

namespace Xenon.Tests.XenonElementsFinderTests
{
	[TestFixture]
	public class XenonElementsFinderTests : BaseXenonIntegrationTest
	{
		public XenonElementsFinderTests()
		{
			XenonTestsResourceLookup.Folder( "XenonElementsFinderTests" );
		}

		private void StartTest( string fileName, Func<XenonTest, XenonTest> testFunc )
		{
			var html = XenonTestsResourceLookup.GetContent( fileName );
			using ( var bt = new BrowserTest( html ) )
			{
				var browser = bt.Start();
				var xenonTest = new XenonTest( browser )
					.GoToUrl( "/" );
				testFunc( xenonTest );
			}
		}

		[Test]
		public void AttributeIs_ElementPresent_ReturnsThatElement()
		{
			StartTest( "FindByAttributeIs", xt =>
				                                xt.Click( x => x.AttributeIs( "data-type", "google" ) )
				                                  .Assert( a => a.UrlContains( "Google" ) ) );
		}

		[Test]
		public void AttributeIs_CombineWithTextIsMethod_ReturnsThatElement()
		{
			StartTest( "FindByAttributeIs", xt =>
				                                xt.Click( x => x.TextIs( "Link" ).AttributeIs( "data-type", "github" ) )
				                                  .Assert( a => a.UrlContains( "Github" ) ) );
		}

		[Test]
		public void AttributeIs_UseAttributeIsMoreThanOneTime_ShouldNotThrowAnException_Issue25()
		{
			StartTest( "Issue25_UsingAttributeIsMethod", xt =>
				                                             xt.Click( x => x.AttributeIs( "class", "btn" ).AttributeIs( "type", "button" ) )
				                                               .Assert( a => a.DialogBoxIsActive() ) );
		}

		[Test]
		public void AttributeIs_WhenMultipleAttributeIsAndTextIsMethodsAreUsed_ShouldFindTheSpecificElement()
		{
			StartTest( "FindByMultipleAttributesAndTextIsMethods", xt =>
				                                                       xt.Click( where => @where
					                                                                 .AttributeIs( "class", "a" )
					                                                                 .AttributeIs( "data-type", "delete" )
					                                                                 .TextIs( "Button" ) )
				                                                         .Assert( a => a.ContainsElement( where => @where.TextIs( "Three" ) ) ) );
		}

	    [Test]
	    public void TextIs_WhenContainsWhiteSpace_ShouldFindTheElement_Issue46()
	    {
	        StartTest( "Issue46_IgnoreWhiteSpace", xt =>
	                                 xt.Assert( a => a.ContainsElement( where => where.TextIs( "Hello World" ) ) ) );
	    }

		[Test]
		public void CriteriaDetails_WithAttributeIsCriteria_DisplaysAttributeCriteriaText()
		{
			var finder = new XenonElementsFinder( null );
			finder.AttributeIs( "Hello", "World" );
			Assert.AreEqual( "[@Hello='World']", finder.CriteriaDetails() );
		}

		[Test]
		public void CriteriaDetails_WithTextIsCriteria_DisplaysTextIsCriteriaText()
		{
			var finder = new XenonElementsFinder( null );
			finder.TextIs( "Hello World" );
			Assert.AreEqual( "(//input[@value='Hello World' and (@type='submit' or @type='button' or @type= 'reset' ) ] | //*[normalize-space(text()) = normalize-space('Hello World')])", finder.CriteriaDetails() );
		}

		[Test]
		public void CssClassIs_ElementsPresent_ReturnsThatElement()
		{
			StartTest( "FindByCssClassIs", xt =>
				                               xt.Click( where => where.CssClassIs( "active" ) )
				                                 .Assert( a => a.UrlContains( "Yahoo" ) ) );
		}

		[Test]
		public void CssClassIs_ElementContainsMultipleCssClasses_ReturnElementsWhichContainsThatCssClass()
		{
			StartTest( "FindByCssClassIs", xt =>
				                               xt.Click( where => where.CssClassIs( "hub" ) )
				                                 .Assert( a => a.UrlContains( "Github" ) ) );
		}

		[Test]
		public void CssClassIs_WhenMultipleCriteriaAreUsed_ReturnElementWhichMeetAllCriteria()
		{
			StartTest( "FindByCssClassIs", xt =>
				                               xt.Click( where => where.TextIs( "Link" ).CssClassIs( "offline" ) )
				                                 .Assert( a => a.UrlContains( "Google" ) ) );
		}

		[Test]
		public void ContainsText_ButtonValueContainsText_ReturnsThatElement()
		{
			const string expected = "Worked";
			StartTest( "FindByContainsText", xt =>

							xt
								.Assert( x => x.PageDoesNotContain( expected ) )
								.Click( where => where.ContainsText( "Add an item" ) )
								.Assert( x => x.DialogBoxIsNotActive().PageContains( expected ) )
								 );
		}

		[Test]
		public void ContainsText_DivContainsText_ReturnsThatElement()
		{
			StartTest( "FindByContainsText", xt =>
							xt.Assert( x => x.ContainsElement( where => where.ContainsText( "Hello" ) ) ) );
		}
	}
}