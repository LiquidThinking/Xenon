using NUnit.Framework;

namespace Xenon.Tests.CancelDialogBoxTests
{
	[TestFixture]
	public class XenonScreenCancelDialogBoxTests : BaseCancelDialogBoxTests<DummyXenonScreen>
	{
		protected override DummyXenonScreen CreateInstance( IXenonBrowser browser )
		{
			return new DummyXenonScreen( browser );
		}
	}
}