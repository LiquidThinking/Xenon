using NUnit.Framework;
using Xenon.Tests.Integration;

namespace Xenon.Tests.EnterTextInDialogBoxTests
{
	public abstract class BaseEnterTextInDialogBoxTests<T> : BaseXenonIntegrationTest where T : BaseXenonTest<T>
	{
		protected abstract T CreateInstance( IXenonBrowser browser );

		protected BaseEnterTextInDialogBoxTests()
		{
			XenonTestsResourceLookup.Folder( "SharedResources" );
		}

		[Test]
		public void EnterTextInDialogBox_PromptDialogBoxIsActive_CanEnterTextInIt()
		{
			const string text = "Test1234";

			var html = XenonTestsResourceLookup.GetContent( "PageWithDialogBoxes" );
			using ( var bt = new BrowserTest( html ) )
			{
				var browser = bt.Start();
				CreateInstance( browser )
					.GoToUrl( "/" )
					.Click( where => where.TextIs( "Prompt Box Enter Text" ) )
					.EnterTextInDialogBox( text )
					.ClickDialogBox()
					.Assert( a => a.ContainsElement( where => where.TextIs( text ) ) );
			}
		}
	}
}