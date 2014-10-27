using Moq;
using NUnit.Framework;

namespace Xenon.Tests.AssertTests
{
	[TestFixture]
	public class XenonScreenAssertTests : BaseAssertTests<DummyXenonScreen>
	{
		protected override BaseXenonTest<DummyXenonScreen> CreateInstance( Mock<IXenonBrowser> browser, XenonTestOptions xenonTestOptions )
		{
			return new DummyXenonScreen( browser.Object, xenonTestOptions );
		}
	}
}