namespace Xenon.Tests
{
	public class DummyXenonScreen : XenonScreen<DummyXenonScreen>
	{
		public DummyXenonScreen( IXenonBrowser xenonBrowser ) : base( xenonBrowser ) { }
		public DummyXenonScreen( IXenonBrowser browser, XenonTestOptions options ) : base( browser, options ) { }
	}
}