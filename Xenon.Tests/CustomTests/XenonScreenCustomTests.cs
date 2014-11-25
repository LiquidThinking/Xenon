using NUnit.Framework;

namespace Xenon.Tests.CustomTests
{
	[TestFixture]
	public class XenonScreenCustomTests : BaseCustomTests<DummyXenonScreen>
	{
		protected override DummyXenonScreen CreateInstance( IXenonBrowser browser )
		{
			return new DummyXenonScreen( browser );
		}
	}
}