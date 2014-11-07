using NUnit.Framework;

namespace Xenon.Tests.SwitchToWindowTests
{
	[TestFixture]
	public class XenonScreenSwitchToWindowTests : BaseSwitchToWindowTests<DummyXenonScreen>
	{
		protected override BaseXenonTest<DummyXenonScreen> CreateInstance( IXenonBrowser browser )
		{
			return new DummyXenonScreen( browser );
		}
	}
}
