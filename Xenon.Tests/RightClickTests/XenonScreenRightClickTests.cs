using Moq;
using NUnit.Framework;

namespace Xenon.Tests.RightClickTests
{
	[TestFixture]
	public class XenonScreenRightClickTests : BaseRightClickTests<DummyXenonScreen>
	{
		protected override BaseXenonTest<DummyXenonScreen> CreateInstance( Mock<IXenonBrowser> browser )
		{
			return new DummyXenonScreen( browser.Object );
		}
	}
}