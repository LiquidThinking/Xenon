using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Xenon.Tests.Integration;

namespace Xenon.Tests.DatePickerTests
{
	public abstract class BaseDatePickerTests<T> : BaseXenonIntegrationTest where T : BaseXenonTest<T>
	{
		protected abstract BaseXenonTest<T> CreateInstance(IXenonBrowser browser);

		protected BaseDatePickerTests()
		{
			XenonTestsResourceLookup.Folder("DatePickerTests");
		}

		[Test]
		public void SelectDate_WhenNativeBrowserDatePicker_CanSetValue()
		{
			var html = XenonTestsResourceLookup.GetContent("VanillaDatePicker");
			using (var browserTest = new BrowserTest(html))
			{
				var date = new DateTime(2015, 1, 1).ToString("yyyy-MM-dd");

				CreateInstance(browserTest.Start())
					.GoToUrl("/")
					.EnterText("input[name=\"date\"]", date)
					.Click(x => x.TextIs("Submit"));

				Assert.AreEqual(date, GetPostedDateValue());

				string GetPostedDateValue()
				{
					var postResult = browserTest.GetPostResult();
					return postResult["date"];
				}
			}
		}
	}
}