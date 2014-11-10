using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Xenon.Tests.ExtensionMethods;
using Xenon.Tests.Integration;

namespace Xenon.Tests.SelectListTests
{
    public abstract class BaseSelectListTests<T> : BaseXenonTest where T : BaseXenonTest<T>
    {
        protected abstract BaseXenonTest<T> CreateInstance(IXenonBrowser browser);

        [Test]
        public void SelectList_WhenCalled_SelectsValue()
        {
            using (
                var browserTest =
                    new BrowserTest(GetEmbeddedResource("Xenon.Tests.SelectListTests.SelectListTests.html")))
            {
                var browser = browserTest.Start();

                CreateInstance(browser)
                    .GoToUrl("/")
                    .SelectList("[name='select-list']", "Second")
                    .Click(where => where.TextIs("Submit"));

                var postResult = browserTest.GetPostResult();

                Assert.AreEqual("2", postResult["select-list"]);
            }
        }

        [Test]
        public void SelectList_WhenDelayBeforeResultsAreDisplayed_SelectsValue()
        {
            using (
                var browserTest =
                    new BrowserTest(GetEmbeddedResource("Xenon.Tests.SelectListTests.SelectListTests.html")))
            {
                var browser = browserTest.Start();

                CreateInstance(browser)
                    .GoToUrl("/")
                    .SelectList("[name='delayed-select-list']", "Third")
                    .Click(where => where.TextIs("Submit"));

                var postResult = browserTest.GetPostResult();

                Assert.AreEqual("3", postResult["delayed-select-list"]);
            }
        }

        public string GetEmbeddedResource(string path)
        {
            using (var reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(path)))
            {
                return reader.ReadToEnd();
            }
        }
    }


}
