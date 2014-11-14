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

		[Test]
		public void AttributeIs_ElementPresent_ReturnsThatElement()
		{
			using ( var bt = new BrowserTest( XenonTestsResourceLookup.GetContent( "FindByAttributeIs" ) ) )
			{
				var browser = bt.Start();
				var xt = new XenonTest( browser )
					.GoToUrl( "/" )
					.Click( x => x.AttributeIs( "data-type", "google" ) )
					.Assert( a => a.UrlContains( "google" ) );
			}
		}

		[Test]
		public void AttributeIs_CombineWithTextIsMethod_ReturnsThatElement()
		{
			using (var bt = new BrowserTest( XenonTestsResourceLookup.GetContent( "FindByAttributeIs" ) ))
			{
				var browser = bt.Start();
				var xt = new XenonTest( browser )
					.GoToUrl( "/" )
					.Click( x => x.TextIs( "Link" ).AttributeIs( "data-type", "github" ) )
					.Assert( a => a.UrlContains( "github.com" ) );
			}

		}
	}
}