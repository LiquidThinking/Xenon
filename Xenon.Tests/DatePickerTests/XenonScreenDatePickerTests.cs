using NUnit.Framework;

namespace Xenon.Tests.DatePickerTests
{
	[TestFixture]
	public class XenonScreenDatePickerTests : BaseDatePickerTests<DummyXenonScreen>
	{
		protected override BaseXenonTest<DummyXenonScreen> CreateInstance( IXenonBrowser browser, string dateFormat = null )
		{
			return new DummyXenonScreen(
				browser,
				string.IsNullOrEmpty( dateFormat )
					? XenonTestOptions.Options
					: XenonTestOptions.Options.Clone( options => options.DateFormat = dateFormat ) );
		}
	}
}