namespace Xenon.Tests.AssertTests.Integration
{
    public class XenonTestAssertTests : BaseAssertTests<XenonTest>
    {
        protected override XenonTest CreateInstance( IXenonBrowser browser )
        {
            return new XenonTest( browser );
        }
    }
}
