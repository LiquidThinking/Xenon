using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xenon.Tests.DoesNotContainTests
{
	public class XenonTestDoesNotContainTests : BaseDoesNotContainTests<XenonTest>
	{
		protected override BaseXenonTest<XenonTest> CreateInstance( IXenonBrowser browser )
		{
			return new XenonTest( browser );
		}
	}
}