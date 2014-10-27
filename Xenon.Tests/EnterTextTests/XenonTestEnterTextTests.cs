using Moq;
using NUnit.Framework;

namespace Xenon.Tests.EnterTextTests
{
	[TestFixture]
	public class XenonTestEnterTextTests : BaseEnterTextTests<XenonTest>
	{
		protected override BaseXenonTest<XenonTest> CreateInstance( Mock<IXenonBrowser> browser )
		{
			return new XenonTest( browser.Object );
		}
	}
}
