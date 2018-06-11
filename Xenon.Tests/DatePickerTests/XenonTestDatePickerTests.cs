using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Xenon.Tests.DatePickerTests
{
	[TestFixture]
	public class XenonTestDatePickerTests : BaseDatePickerTests<XenonTest>
	{
		protected override BaseXenonTest<XenonTest> CreateInstance( IXenonBrowser browser )
		{
			return new XenonTest( browser );
		}
	}
}