using NUnit.Framework;

namespace Xenon.Tests.ClickDialogBoxTests
{
	[TestFixture]
	public class XenonScreenClickDialogBoxTests : BaseClickDialogBoxTests<DummyXenonScreen>
	{
		protected override BaseXenonTest<DummyXenonScreen> CreateInstance( IXenonBrowser browser )
		{
			return new DummyXenonScreen( browser );
		}
	}
}