using NUnit.Framework;

namespace Xenon.Tests.FailureTests
{
	[TestFixture]
	public class XenonTestFailureTests : BaseFailureTests<XenonTest>
	{
		protected override BaseXenonTest<XenonTest> CreateInstance( IXenonBrowser browser )
		{
			return new XenonTest( browser );
		}
	}
}