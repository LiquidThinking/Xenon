using Moq;
using NUnit.Framework;

namespace Xenon.Tests.ClickTests
{
	[TestFixture]
	public class XenonScreenClickTests : BaseClickTests<DummyXenonScreen>
	{
		protected override BaseXenonTest<DummyXenonScreen> CreateInstance( Mock<IXenonBrowser> browser )
		{
			return new DummyXenonScreen( browser.Object );
		}
	}
}