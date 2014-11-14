using NUnit.Framework;
using Xenon.Tests.Integration;

namespace Xenon.Tests.ClickDialogBoxTests
{
	public abstract class BaseClickDialogBoxTests<T> : BaseXenonIntegrationTest where T : BaseXenonTest<T>
	{
		protected abstract BaseXenonTest<T> CreateInstance( IXenonBrowser browser );

		protected BaseClickDialogBoxTests()
		{
			XenonTestsResourceLookup.Folder( "ClickDialogBoxTests" );
		}

		private void ArrangeActAndAssert( string linkText, string assertResultContainsText )
		{
			var html = XenonTestsResourceLookup.GetContent( "PageWithDialogBoxes" );
			using ( var bt = new BrowserTest( html ) )
			{
				var browser = bt.Start();
				CreateInstance( browser )
					.GoToUrl( "/" )
					.Click( where => where.TextIs( linkText ) )
					.ClickDialogBox()
					.Assert( a => a.ContainsElement( where => where.TextIs( assertResultContainsText ) ) );
			}
		}

		[Test]
		public void ClickDialogBox_AlertDialogBoxIsActive_ClickIt()
		{
			ArrangeActAndAssert( "Alert Box", "Clicked Alert Dialog box" );
		}

		[Test]
		public void ClickDialogBox_ConfirmDialogBoxIsActive_ClickOk()
		{
			ArrangeActAndAssert( "Confirm Box", "Clicked Confirm Dialog Box" );
		}

		[Test]
		public void ClickDialogBox_PromptDialogBoxIsActive_ClickIt()
		{
			ArrangeActAndAssert( "Prompt Box", "Clicked Prompt Dialog Box" );
		}
	}
}