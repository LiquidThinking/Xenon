using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using Xenon.Tests.Intergration;

namespace Xenon.Tests.SwitchToWindowTests
{
	public abstract class BaseSwitchToWindowTests<T> : BaseXenonTest where T : BaseXenonTest<T>
	{
		protected abstract BaseXenonTest<T> CreateInstance( IXenonBrowser browser );

		[SetUp]
		public void Setup()
		{
			XenonTestOptions.Options = new XenonTestOptions
			{
				AssertMethod = Assert.IsTrue,
			};
		}

		private static string GeEmbeddedResourceContent( string fileName )
		{
			const string resourceIdentifierFormat = "Xenon.Tests.SwitchToWindowTests.{0}.html";
			string resourceIdentifier = String.Format( resourceIdentifierFormat, fileName );

			var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream( resourceIdentifier );
			var content = new StreamReader( stream ).ReadToEnd();
			return content;
		}

		[Test]
		public void CanSwitchToNextWindow()
		{
			var html = GeEmbeddedResourceContent( "SwitchBetweenMultipleWindows" );
			using ( var browserTest = new BrowserTest( html ) )
			{
				var browser = browserTest.Start();
				try
				{
					CreateInstance( browser )
						.GoToUrl( "/" )
						.Click( where => where.TextIs( "Google" ) )
						.SwitchToWindow( a => a.PageContains( "I'm Feeling Lucky" ) )
						.Assert( a => a.PageContains( "I'm Feeling Lucky" ) )
						.SwitchToWindow( a => a.PageContains( "Test Page" ) )
						.Click( where => where.TextIs( "Yahoo" ) )
						.SwitchToWindow( a => a.PageContains( "Search web" ) )
						.Assert( a => a.PageContains( "Search web" ) )
						.SwitchToWindow( a => a.PageContains( "Test Page" ) )
						.Assert( a => a.PageContains( "Test Page" ) );
				} finally
				{
					browser.Quit();
				}
			}
		}

		[Test]
		public void CanCloseAndSwitchToAnotherWindow()
		{
			var html = GeEmbeddedResourceContent( "SwitchBetweenMultipleWindows" );
			using ( var browserTest = new BrowserTest( html ) )
			{
				var browser = browserTest.Start();
				try
				{
					CreateInstance( browser )
						.GoToUrl( "/" )
						.Click( where => where.TextIs( "Google" ) )
						.SwitchToWindow( a => a.PageContains( "I'm Feeling Lucky" ) )
						.Assert( a => a.PageContains( "I'm Feeling Lucky" ) )
						.CloseCurrentAndSwitchToWindow(a => a.PageContains( "Test Page" ))
						.Assert( a => a.PageContains( "Test Page" ) );
				} finally
				{
					browser.Quit();
				}
			}
		}
	}
}