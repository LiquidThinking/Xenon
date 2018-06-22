using NUnit.Framework;

namespace Xenon.Tests.MissingElementsTests
{
	[TestFixture]
	public class XenonTestMissingElementsTest : BaseMissingElementsTest<XenonTest>
	{
		protected override BaseXenonTest<XenonTest> CreateInstance( IXenonBrowser browser )
		{
			return new XenonTest( browser );
		}
	}
}