using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Xenon.Tests.Integration;

namespace Xenon.Tests.DatePickerTests
{
	public abstract class BaseDatePickerTests<T> : BaseXenonIntegrationTest where T : BaseXenonTest<T>
	{
		protected abstract BaseXenonTest<T> CreateInstance( IXenonBrowser browser );

		protected BaseDatePickerTests()
		{
			XenonTestsResourceLookup.Folder( "DatePickerTests" );
		}

		private string GetHtml()
		{
			return XenonTestsResourceLookup.GetContent( "DatePicker" );
		}

		[Test]
		public void EnterText_WhenTextElementIsDatePicker_ThrowsException()
		{
			using ( var browserTest = new BrowserTest( GetHtml() ) )
			{
				Assert.Throws<IncorrectInputElementTypeException>( () => {
																		CreateInstance( browserTest.Start() )
																			.GoToUrl( "/" )
																			.EnterText( "input[type=\"date\"]", new DateTime( 2000, 1, 1 ).ToString() );
																	} );
			}
		}

		[TestCase( BrowserType.Chrome )]
		[TestCase( BrowserType.Firefox )]
		public void EnterDateIntoDatePicker_DoesSetValue( BrowserType browserType )
		{
			using ( var browserTest = new BrowserTest( GetHtml() ) )
			{
				var theFifthOfNovember = new DateTime( 1605, 11, 5 );

				CreateInstance( browserTest.Start( browserType ) )
					.GoToUrl( "/" )
					.EnterDate( "input[name=\"date\"]", theFifthOfNovember )
					.Click( x => x.TextIs( "Submit" ) );

				Assert.AreEqual( theFifthOfNovember, GetPostedDateValue() );

				DateTime GetPostedDateValue()
				{
					var postResult = browserTest.GetPostResult();
					return DateTime.Parse( postResult[ "date" ] );
				}
			}
		}
	}
}