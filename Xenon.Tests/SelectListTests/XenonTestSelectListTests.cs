using NUnit.Framework;

namespace Xenon.Tests.SelectListTests
{
	[TestFixture]
    public class XenonTestSelectListTests : BaseSelectListTests<XenonTest>
	{
        protected override BaseXenonTest<XenonTest> CreateInstance(IXenonBrowser browser)
		{
			return new XenonTest( browser ); 
		}
	}
}