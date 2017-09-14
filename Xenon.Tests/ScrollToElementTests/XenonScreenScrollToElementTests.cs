using NUnit.Framework;
using Xenon.Tests.ClearTests;

namespace Xenon.Tests.ScrollToElementTests
{
	[TestFixture]
	public class XenonScreenScrollToElementTests : BaseScrollToElementTests<DummyXenonScreen>
	{
		protected override BaseXenonTest<DummyXenonScreen> CreateInstance( IXenonBrowser browser )
		{
			return new DummyXenonScreen( browser );
		}
	}
}