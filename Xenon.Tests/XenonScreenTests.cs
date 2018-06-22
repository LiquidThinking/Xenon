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
			public ScreenA( IXenonBrowser browser, XenonTestOptions options = null ) : base( browser, options ) {}
		}

		public class ScreenB : DummyScreenHelper<ScreenB>
		{
			public ScreenB( IXenonBrowser browser, XenonTestOptions options = null ) : base( browser, options ) {}
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


		public class FirstScreenWhenNextScreenHasOnly1Constructor : DummyScreenHelper<FirstScreenWhenNextScreenHasOnly1Constructor>
		{
			public FirstScreenWhenNextScreenHasOnly1Constructor( IXenonBrowser browser, XenonTestOptions options = null ) : base( browser, options ) {}
		}

		public class SecondScreenWhenNextScreenHasOnly1Constructor : DummyScreenHelper<SecondScreenWhenNextScreenHasOnly1Constructor>
		{
			public SecondScreenWhenNextScreenHasOnly1Constructor( IXenonBrowser browser, XenonTestOptions options = null ) : base( browser, options ) {}
		}

		[Test]
		public void Switch_WhenNextScreenHasOnly1Constructor_CanSwitchToNextScreen()
		{
			var browser = new Mock<IXenonBrowser>();
			var options = new XenonTestOptions();

			var firstScreen = new FirstScreenWhenNextScreenHasOnly1Constructor( browser.Object, options );
			Assert.DoesNotThrow( () => firstScreen.Switch<SecondScreenWhenNextScreenHasOnly1Constructor>() );
		}
	}
}