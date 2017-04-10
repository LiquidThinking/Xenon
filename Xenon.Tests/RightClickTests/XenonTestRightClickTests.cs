using Moq;
using NUnit.Framework;

namespace Xenon.Tests.RightClickTests
{
	[TestFixture]
	public class XenonTestRightClickTests : BaseRightClickTests<XenonTest>
	{
		protected override BaseXenonTest<XenonTest> CreateInstance( Mock<IXenonBrowser> browser )
		{
			return new XenonTest( browser.Object );
		}
	}
}