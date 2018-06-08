using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Xenon.Tests.DatePickerTests
{
	[TestFixture]
	public class XenonScreenDatePickerTests : BaseDatePickerTests<DummyXenonScreen>
	{
		protected override BaseXenonTest<DummyXenonScreen> CreateInstance(IXenonBrowser browser)
		{
			return new DummyXenonScreen(browser);
		}
	}
}
