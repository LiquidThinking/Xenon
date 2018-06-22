using NUnit.Framework;

namespace Xenon.Tests.FailureTests
{
	[TestFixture]
	public class XenonScreenFailureTests : BaseFailureTests<DummyXenonScreen>
	{
		protected override BaseXenonTest<DummyXenonScreen> CreateInstance( IXenonBrowser browser )
		{
			return new DummyXenonScreen( browser );
		}
	}
}
