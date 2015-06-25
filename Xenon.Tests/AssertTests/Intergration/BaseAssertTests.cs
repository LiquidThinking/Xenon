using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Xenon.Tests.Integration;

namespace Xenon.Tests.AssertTests.Intergration
{
    public abstract class BaseAssertTests<T> : BaseXenonIntegrationTest where T : BaseXenonTest<T>
    {
        protected abstract T CreateInstance( IXenonBrowser browser );

        protected BaseAssertTests()
        {
            XenonTestsResourceLookup.Folder( "AssertTests" );
        }

        [Test]
        public void AssertPageContains_WithElementsOnPage_ChecksInputValues()
        {
            var html = XenonTestsResourceLookup.GetContent( "PageWithInputs" );
            using ( var bt = new BrowserTest( html ) )
            {
                var browser = bt.Start();
                CreateInstance( browser )
                    .GoToUrl( "/" )
                    .Click( where => where.TextIs( "Fill Form" ) )
                    .Assert( a => a.PageContains( "First Name" ) )
                    .Assert( a => a.PageContains( "Description Content" ) );
            }
        }
    }
}