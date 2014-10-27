using Moq;
using NUnit.Framework;

namespace Xenon.Tests.EnterTextTests
{
	[TestFixture]
	public class XenonScreenEnterTextTests : BaseEnterTextTests<DummyXenonScreen>
	{
		protected override BaseXenonTest<DummyXenonScreen> CreateInstance( Mock<IXenonBrowser> browser )
		{
			return new DummyXenonScreen( browser.Object );
		}
	}
}