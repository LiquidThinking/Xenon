using NUnit.Framework;

namespace Xenon.Tests.MissingElementsTests
{
	[TestFixture]
	public class XenonScreenMissingElementsTest : BaseMissingElementsTest<DummyXenonScreen>
	{
		protected override BaseXenonTest<DummyXenonScreen> CreateInstance( IXenonBrowser browser )
		{
			return new DummyXenonScreen( browser );
		}
	}
}
