using NUnit.Framework;
using Xenon.Tests.ClearTests;

namespace Xenon.Tests.ScrollToElementTests
{
	[TestFixture]
	public class XenonTestScrollToElementTests : BaseScrollToElementTests<XenonTest>
	{
		protected override BaseXenonTest<XenonTest> CreateInstance( IXenonBrowser browser )
		{
			return new XenonTest( browser );
		}
	}
}