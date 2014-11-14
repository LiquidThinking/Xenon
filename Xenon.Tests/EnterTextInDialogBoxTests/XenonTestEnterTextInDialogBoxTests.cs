using NUnit.Framework;

namespace Xenon.Tests.EnterTextInDialogBoxTests
{
	[TestFixture]
	public class XenonTestEnterTextInDialogBoxTests : BaseEnterTextInDialogBoxTests<XenonTest>
	{
		protected override XenonTest CreateInstance( IXenonBrowser browser )
		{
			return new XenonTest( browser );
		}
	}
}