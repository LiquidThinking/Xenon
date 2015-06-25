using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace Xenon.Tests.AssertTests.Intergration
{
    public class XenonTestAssertTests : BaseAssertTests<XenonTest>
    {
        protected override XenonTest CreateInstance( IXenonBrowser browser )
        {
            return new XenonTest( browser );
        }
    }
}
