using Moq;
using NUnit.Framework;

namespace Xenon.Tests.GoToUrlTests
{
	[TestFixture]
	public class XenonTestGoToUrl : BaseGoToUrlTests<XenonTest>
	{
		protected override BaseXenonTest<XenonTest> CreateInstance( Mock<IXenonBrowser> browser )
		{
			return new XenonTest( browser.Object );
		}
	}
}