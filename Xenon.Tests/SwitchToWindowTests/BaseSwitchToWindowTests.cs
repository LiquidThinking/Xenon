using System.Threading;
using NUnit.Framework;
using Xenon.Tests.Integration;

namespace Xenon.Tests.SwitchToWindowTests
{
	public abstract class BaseSwitchToWindowTests<T> : BaseXenonIntegrationTest where T : BaseXenonTest<T>
	{
		protected abstract BaseXenonTest<T> CreateInstance( IXenonBrowser browser );

		public BaseSwitchToWindowTests()
		{
			XenonTestsResourceLookup.Folder( "SwitchToWindowTests" );
		}

		[SetUp]
		public void Setup()
		{
			XenonTestOptions.Options = new XenonTestOptions
			{
				AssertMethod = Assert.IsTrue,
			};
		}

		[Test]
		public void CanSwitchToNextWindow()
		{
			var html = XenonTestsResourceLookup.GetContent( "SwitchBetweenMultipleWindows" );
			using ( var browserTest = new BrowserTest( html ) )
			{
				var browser = browserTest.Start();
				CreateInstance( browser )
					.GoToUrl( "/" )
					.Click( where => where.TextIs( "Google" ) )
					.SwitchToWindow( a => a.PageContains( "I'm Feeling Lucky" ) )
					.Assert( a => a.PageContains( "I'm Feeling Lucky" ) )
					.SwitchToWindow( a => a.PageContains( "Test Page" ) )
					.Click( where => where.TextIs( "Yahoo" ) )
					.SwitchToWindow( a => a.PageContains( "Yahoo" ) )
					.Assert( a => a.PageContains( "Yahoo" ) )
					.SwitchToWindow( a => a.PageContains( "Test Page" ) )
					.Assert( a => a.PageContains( "Test Page" ) );
			}
		}

		[Test]
		public void CanCloseAndSwitchToAnotherWindow()
		{
			var html = XenonTestsResourceLookup.GetContent( "SwitchBetweenMultipleWindows" );
			using ( var browserTest = new BrowserTest( html ) )
			{
				var browser = browserTest.Start();

				CreateInstance( browser )
					.GoToUrl( "/" )
					.Click( where => where.TextIs( "Google" ) )
					.SwitchToWindow( a => a.PageContains( "I'm Feeling Lucky" ) )
					.Assert( a => a.PageContains( "I'm Feeling Lucky" ) )
					.CloseCurrentAndSwitchToWindow( a => a.PageContains( "Test Page" ) )
					.Assert( a => a.PageContains( "Test Page" ) );
			}
		}
	}
}