using Moq;
using NUnit.Framework;

namespace Xenon.Tests.GoToUrlTests
{
	[TestFixture]
	public class XenonScreenGoToUrl : BaseGoToUrlTests<DummyXenonScreen>
	{
		protected override BaseXenonTest<DummyXenonScreen> CreateInstance( Mock<IXenonBrowser> browser )
		{
			return new DummyXenonScreen( browser.Object );
		}

	}
}