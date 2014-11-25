using NUnit.Framework;

namespace Xenon.Tests.CustomTests
{
	[TestFixture]
	public class XenonTestCustomTests : BaseCustomTests<XenonTest>
	{
		protected override XenonTest CreateInstance( IXenonBrowser browser )
		{
			return new XenonTest( browser );
		}
	}
}