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
				                                  .Assert( a => a.UrlContains( "google" ) ) );
		}

		[Test]
		public void AttributeIs_CombineWithTextIsMethod_ReturnsThatElement()
		{
			StartTest( "FindByAttributeIs", xt =>
				                                xt.Click( x => x.TextIs( "Link" ).AttributeIs( "data-type", "github" ) )
				                                  .Assert( a => a.UrlContains( "github.com" ) ) );
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
	}
}