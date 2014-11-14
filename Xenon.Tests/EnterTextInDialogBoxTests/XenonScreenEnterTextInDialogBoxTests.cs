using NUnit.Framework;

namespace Xenon.Tests.EnterTextInDialogBoxTests
{
	[TestFixture]
	public class XenonScreenEnterTextInDialogBoxTests : BaseEnterTextInDialogBoxTests<DummyXenonScreen>
	{
		protected override DummyXenonScreen CreateInstance( IXenonBrowser browser )
		{
			return new DummyXenonScreen( browser );
		}
	}
}