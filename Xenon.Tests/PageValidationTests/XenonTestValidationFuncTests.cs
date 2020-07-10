using NUnit.Framework;

namespace Xenon.Tests.PageValidationTests
{
	[TestFixture]
	public class XenonTestValidationFuncTests : BasePageValidationFuncTests<XenonTest>
	{
		protected override XenonTest CreateInstance( IXenonBrowser browser )
		{
			return new XenonTest( browser, Options );
		}
	}
}