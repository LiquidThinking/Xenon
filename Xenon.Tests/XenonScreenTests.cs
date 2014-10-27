using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

namespace Xenon.Tests
{
	[TestFixture]
	public class XenonScreenTests
	{
		public class DummyScreenHelper<T> : XenonScreen<T> where T : XenonScreen<T>
		{
			private readonly IXenonBrowser _browser;
			private readonly XenonTestOptions _options;

			public DummyScreenHelper( IXenonBrowser browser ) : base( browser )
			{
				_browser = browser;
			}

			public DummyScreenHelper( IXenonBrowser browser, XenonTestOptions options ) : base( browser, options )
			{
				_browser = browser;
				_options = options;
			}

			public IXenonBrowser GetBrowser()
			{
				return _browser;
			}

			public XenonTestOptions GetTestOptions()
			{
				return _options;
			}
		}

		public class ScreenA : DummyScreenHelper<ScreenA>
		{
			public ScreenA( IXenonBrowser xenonBrowser ) : base( xenonBrowser ) {}
			public ScreenA( IXenonBrowser browser, XenonTestOptions options ) : base( browser, options ) {}
		}

		public class ScreenB : DummyScreenHelper<ScreenB>
		{
			public ScreenB( IXenonBrowser xenonBrowser ) : base( xenonBrowser ) {}
			public ScreenB( IXenonBrowser browser, XenonTestOptions options ) : base( browser, options ) {}
		}

		[Test]
		public void Switch_WhenProvidedScreenType_ReturnsThatScreenTypeWithSameBrowserAndOptions()
		{
			var browser = new Mock<IXenonBrowser>();
			var options = new XenonTestOptions();

			var screenA = new ScreenA( browser.Object, options );
			var screenB = screenA.Switch<ScreenB>();

			Assert.AreEqual( browser.Object, screenB.GetBrowser() );
			Assert.AreEqual( options, screenB.GetTestOptions() );
		}
	}
}
