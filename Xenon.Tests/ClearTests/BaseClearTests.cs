using NUnit.Framework;
using Xenon.Tests.Integration;

namespace Xenon.Tests.ClearTests
{
	public abstract class BaseClearTests<T> : BaseXenonIntegrationTest where T : BaseXenonTest<T>
	{
		protected abstract BaseXenonTest<T> CreateInstance( IXenonBrowser browser );

        public BaseClearTests()
		{
            XenonTestsResourceLookup.Folder("ClearTests");
		}

		[Test]
		public void ClickDialogBox_AlertDialogBoxIsActive_ClickIt()
		{
            var html = XenonTestsResourceLookup.GetContent("Clear");
            using (var bt = new BrowserTest(html))
            {
                var browser = bt.Start();
                CreateInstance(browser)
                    .GoToUrl("/")
                    .Clear("#text-box")
                    .Click(where => where.TextIs("Submit"));

                Assert.AreEqual("", bt.GetPostResult()["text-box"]);
            }
		}
	}
}