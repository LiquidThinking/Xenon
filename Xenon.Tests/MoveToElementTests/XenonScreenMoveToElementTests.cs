using NUnit.Framework;
using Xenon.Tests.ClearTests;

namespace Xenon.Tests.MoveToElementTests
{
	[TestFixture]
	public class XenonScreenMoveToElementTests : BaseMoveToElementTests<DummyXenonScreen>
	{
		protected override BaseXenonTest<DummyXenonScreen> CreateInstance( IXenonBrowser browser )
		{
			return new DummyXenonScreen( browser );
		}
	}
}