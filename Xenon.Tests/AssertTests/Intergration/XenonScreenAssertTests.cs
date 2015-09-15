using Moq;
using NUnit.Framework;

namespace Xenon.Tests.AssertTests.Intergration
{
    [TestFixture]
    public class XenonScreenAssertTests : BaseAssertTests<DummyXenonScreen>
    {
        protected override DummyXenonScreen CreateInstance( IXenonBrowser browser )
        {
            return new DummyXenonScreen( browser );
        }
    }
}
