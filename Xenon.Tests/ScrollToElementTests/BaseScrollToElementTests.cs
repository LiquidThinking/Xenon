using NUnit.Framework;
using Xenon.Tests.Integration;

namespace Xenon.Tests.ScrollToElementTests
{
	public abstract class BaseScrollToElementTests<T> : BaseXenonIntegrationTest where T : BaseXenonTest<T>
	{
		protected abstract BaseXenonTest<T> CreateInstance( IXenonBrowser browser );

        public BaseScrollToElementTests()
		{
            XenonTestsResourceLookup.Folder("ScrollToElementTests");
		}

		[Test]
		public void ClickDialogBox_AlertDialogBoxIsActive_ClickIt()
		{
            var html = XenonTestsResourceLookup.GetContent("ScrollToElement");
            using (var bt = new BrowserTest(html))
            {
                var browser = bt.Start();
                CreateInstance(browser)
                    .GoToUrl("/")
                    .Click(where => where.TextIs("Click"));

                Assert.AreEqual("worked", bt.GetPostResult()["temp"]);
            }
		}
	}
}