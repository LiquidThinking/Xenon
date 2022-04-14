﻿using System.Linq;
using NUnit.Framework;
using Xenon.Tests.Integration;

namespace Xenon.Tests.PageValidationTests
{
	public abstract class BasePageValidationFuncTests<T> : BaseXenonIntegrationTest where T : BaseXenonTest<T>
	{
		private const string ErrorMessage = "Custom Validation Failed";

		public XenonTestOptions Options { get; }
			= new XenonTestOptions
			{
				AssertMethod = Assert.IsTrue,
				PageValidationFunc = page =>
					page.Source.Contains( "<h1>Error</h1>" )
						? ErrorMessage
						: null
			};

		protected BasePageValidationFuncTests()
		{
			XenonTestsResourceLookup.Folder( "PageValidationTests" );
		}

		protected abstract T CreateInstance( IXenonBrowser browser );

		[Test]
		public void GoToUrl_CustomValidationFails_FailsTest()
		{
			using ( var browserTest = new BrowserTest(
				XenonTestsResourceLookup
					.GetContent(
						htmlFileName: "PageWithErrorHeader" ) ) )
			{
				var exception = Assert
					.Throws<AssertionException>(
						() => CreateInstance(
							browserTest.Start() ).GoToUrl( "/" ) );

				Assert.IsTrue( exception.Message.Contains( ErrorMessage ) );
			}
		}


		[Test]
		public void Assertion_CustomValidationFails_FailsTest()
		{
			using ( var browserTest = new BrowserTest(
				XenonTestsResourceLookup
					.GetContent(
						htmlFileName: "PageWithoutErrorHeader" ) ) )
			{
				var loadedPage = CreateInstance( browserTest.Start() ).GoToUrl( "/" );

				var exception = Assert
					.Throws<AssertionException>(
						() =>
						{
							loadedPage
								//trigger the bad page state
								.Click( "button" )
								.Assert( assertion => assertion
									.PageContains( "Make Error" ) );
						} );

				Assert.IsTrue( exception.Message.Contains( ErrorMessage ) );
			}
		}

		[Test]
		public void GoToUrl_CustomValidationPasses_DoesNotFailTest()
		{
			using ( var browserTest = new BrowserTest(
				XenonTestsResourceLookup
					.GetContent(
						htmlFileName: "PageWithoutErrorHeader" ) ) )
			{
				Assert
					.DoesNotThrow(
						() => CreateInstance(
							browserTest.Start() ).GoToUrl( "/" ) );
			}
		}

		[Test]
		public void Assertion_CustomValidationPasses_DoesNotFailTest()
		{
			using ( var browserTest = new BrowserTest(
				XenonTestsResourceLookup
					.GetContent(
						htmlFileName: "PageWithoutErrorHeader" ) ) )
			{
				Assert
					.DoesNotThrow(
						() => CreateInstance(
								browserTest.Start() )
							.GoToUrl( "/" )
							.Assert( assertion => assertion
								.PageContains( "200" ) ) );
			}
		}

		[Test]
		public void ActionTriggeringModal_WhileModalActive_CanHandleModal()
		{
			using ( var browserTest = new BrowserTest(
				XenonTestsResourceLookup
					.GetContent(
						htmlFileName: "PageWithoutErrorHeader" ) ) )
			{
				CreateInstance( browserTest.Start() )
					.GoToUrl( "/" )
					.Click( "a" )
					.Assert( assertion => assertion
						.CustomAssertion( browser => browser
							.DialogBoxIsActive() ) );
			}
		}
	}
}