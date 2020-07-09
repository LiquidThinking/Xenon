using NUnit.Framework;
using Xenon.Tests.Integration;

namespace Xenon.Tests.PageValidationTests
{
	public abstract class BasePageValidationFuncTests<T> : BaseXenonIntegrationTest where T : BaseXenonTest<T>
	{
		private const string ErrorMessage = "Custom Validation Failed On Navigation";

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
	}
}