namespace Xenon
{
	public class XenonTest : BaseXenonTest<XenonTest>
	{
		public XenonTest( IXenonBrowser xenonBrowser ) : base( xenonBrowser ) {}
		public XenonTest( IXenonBrowser browser, XenonTestOptions options ) : base( browser, options ) {}
	}
}