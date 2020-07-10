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
		protected override BaseXenonTest<XenonTest> CreateInstance( IXenonBrowser browser, string dateFormat = null )
		{
			return new XenonTest(
				browser,
				string.IsNullOrEmpty( dateFormat ) 
					? XenonTestOptions.Options
					: XenonTestOptions.Options.Clone( options => options.DateFormat = dateFormat ) );
		}
	}
}