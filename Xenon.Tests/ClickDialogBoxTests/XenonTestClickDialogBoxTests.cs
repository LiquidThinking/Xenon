using NUnit.Framework;

namespace Xenon.Tests.ClickDialogBoxTests
{
	[TestFixture]
	public class XenonTestClickDialogBoxTests : BaseClickDialogBoxTests<XenonTest>
	{
		protected override BaseXenonTest<XenonTest> CreateInstance( IXenonBrowser browser )
		{
			return new XenonTest( browser );
		}
	}
}