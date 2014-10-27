using Moq;
using NUnit.Framework;

namespace Xenon.Tests.ClickTests
{
	[TestFixture]
	public class XenonTestClickTests : BaseClickTests<XenonTest>
	{
		protected override BaseXenonTest<XenonTest> CreateInstance( Mock<IXenonBrowser> browser )
		{
			return new XenonTest( browser.Object );
		}
	}
}