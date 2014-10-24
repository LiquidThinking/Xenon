using Moq;

namespace Xenon.Tests
{
	public abstract class BaseXenonTest
	{
		protected static XenonAssertion EmptyAssertion( XenonAssertion xenonAssertion ) => xenonAssertion;

		protected static Mock<IXenonBrowser> SetupBrowser()
		{
			var browser = new Mock<IXenonBrowser>();
			browser.Setup( x => x.RunAssertion( It.IsAny<AssertionFunc>() ) )
			       .Returns<AssertionFunc>( x => x( new XenonAssertion( browser.Object ) ) );

			return browser;
		}

	}
}