using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Xenon.Tests.Integration;

namespace Xenon.Tests.InputTests
{
	public class XenonTestInputTests : BaseXenonIntegrationTest
	{

		[OneTimeSetUp]
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
					.Assert( x => x.CustomAssertion( y => y.FindElementsByCssSelector( String.Format( "input[type='{0}']", inputType ) ).First().Text == expectedValue ) );
			}
		}

	}
}
