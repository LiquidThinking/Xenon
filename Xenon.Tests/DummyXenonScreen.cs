namespace Xenon.Tests
{
	public class DummyXenonScreen : XenonScreen<DummyXenonScreen>
	{
		public DummyXenonScreen( IXenonBrowser browser, XenonTestOptions options = null) : base( browser, options ) { }
	}
}