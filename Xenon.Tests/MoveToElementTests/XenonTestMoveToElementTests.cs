using NUnit.Framework;

namespace Xenon.Tests.MoveToElementTests
{
	[TestFixture]
	public class XenonTestMoveToElementTests : BaseMoveToElementTests<XenonTest>
	{
		protected override BaseXenonTest<XenonTest> CreateInstance( IXenonBrowser browser )
		{
			return new XenonTest( browser );
		}
	}
}