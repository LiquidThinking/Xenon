using System.Linq;
using NUnit.Framework;
using Xenon.Tests.Integration;

namespace Xenon.Tests.InputTests
{
	public class XenonTestInputTests : BaseXenonIntegrationTest
	{

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			XenonTestsResourceLookup.Folder( "InputTests" );
		}

		[TestCase("text", "A")]
		[TestCase( "number", "1" )]
		public void GettingTextOfInput_ReturnsValueOfInput( string inputType, string expectedValue )
		{
			var html = XenonTestsResourceLookup.GetContent( "InputTests" );
			using ( var browserTest = new BrowserTest( html ) )
			{
				var browser = browserTest.Start();

				new DummyXenonScreen( browser )
					.GoToUrl( "/" )
					.Assert( x => InputHasExpectedValue( inputType, expectedValue, x ) );
			}
		}

		private static XenonAssertion InputHasExpectedValue( string inputType, string expectedValue, XenonAssertion xenonAssertion )
		{
			return xenonAssertion.CustomAssertion( y => y.FindElementsByCssSelector( $"input[type='{inputType}']" )
				                               .First().Text == expectedValue );
		}
	}
}
