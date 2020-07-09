using System;
using NUnit.Framework;

namespace Xenon.Tests
{
    [TestFixture]
    public class XenonTestOptionsTests
    {
        [Test]
        public void IfAssertMethodIsNotSet_WhenAccessedShouldShouldThrowException()
        {
            var xenonTestOptions = new XenonTestOptions();

            var exception = Assert.Catch<Exception>(() =>
            {
                var assertMethod = xenonTestOptions.AssertMethod;
            });

            Assert.AreEqual("You must set an AssertMethod", exception.Message);
        }
    }
}
