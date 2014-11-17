using NUnit.Framework;

namespace Xenon.Tests.CancelDialogBoxTests
{
	[TestFixture]
	public class XenonTestCancelDialogBoxTests : BaseCancelDialogBoxTests<XenonTest>
	{
		protected override XenonTest CreateInstance( IXenonBrowser browser )
		{
			return new XenonTest( browser );
		}
	}
}