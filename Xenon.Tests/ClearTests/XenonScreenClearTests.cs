using NUnit.Framework;

namespace Xenon.Tests.ClearTests
{
	[TestFixture]
	public class XenonScreenClearTests : BaseClearTests<DummyXenonScreen>
	{
		protected override BaseXenonTest<DummyXenonScreen> CreateInstance( IXenonBrowser browser )
		{
			return new DummyXenonScreen( browser );
		}
	}
}