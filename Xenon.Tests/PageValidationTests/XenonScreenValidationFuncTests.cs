using NUnit.Framework;

namespace Xenon.Tests.PageValidationTests
{
	[TestFixture]
	public class XenonScreenValidationFuncTests : BasePageValidationFuncTests<DummyXenonScreen>
	{
		protected override DummyXenonScreen CreateInstance( IXenonBrowser browser )
		{
			return new DummyXenonScreen( browser, Options );
		}
	}
}