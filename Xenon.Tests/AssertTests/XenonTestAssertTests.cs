using Moq;
using NUnit.Framework;

namespace Xenon.Tests.AssertTests
{
	[TestFixture]
	public class XenonTestAssertTests : BaseAssertTests<XenonTest>
	{
		protected override BaseXenonTest<XenonTest> CreateInstance( Mock<IXenonBrowser> browser, XenonTestOptions xenonTestOptions )
		{
			return new XenonTest( browser.Object, xenonTestOptions );
		}
	}
}