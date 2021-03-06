﻿using System;
using NUnit.Framework;
using Xenon.Tests.Integration;

namespace Xenon.Tests.DatePickerTests
{
	public abstract class BaseDatePickerTests<T> : BaseXenonIntegrationTest where T : BaseXenonTest<T>
	{
		protected const string ChromeDateFormat = "dd/MM/yyyy";
		protected abstract BaseXenonTest<T> CreateInstance( IXenonBrowser browser, string dateFormat = null );

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

		[TestCase( BrowserType.Chrome, ChromeDateFormat )]
		[TestCase( BrowserType.Firefox, XenonTestOptions.DefaultDateFormat )]
		public void EnterDateIntoDatePicker_DoesSetValue( BrowserType browserType, string dateFormat )
		{
			using ( var browserTest = new BrowserTest( GetHtml() ) )
			{
				var theFifthOfNovember = new DateTime( 1605, 11, 5 );

				CreateInstance( 
						browserTest.Start( browserType ), 
						dateFormat )
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