using NUnit.Framework;

namespace Xenon.Tests.SwitchToWindowTests
{
	[TestFixture]
	public class XenonTestSwitchToWindowTests : BaseSwitchToWindowTests<XenonTest>
	{
		protected override BaseXenonTest<XenonTest> CreateInstance( IXenonBrowser browser )
		{
			return new XenonTest( browser );
		}
	}
}
