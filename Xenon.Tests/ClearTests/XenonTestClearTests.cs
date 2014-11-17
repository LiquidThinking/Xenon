using NUnit.Framework;

namespace Xenon.Tests.ClearTests
{
	[TestFixture]
	public class XenonTestClearTests : BaseClearTests<XenonTest>
	{
		protected override BaseXenonTest<XenonTest> CreateInstance( IXenonBrowser browser )
		{
			return new XenonTest( browser );
		}
	}
}