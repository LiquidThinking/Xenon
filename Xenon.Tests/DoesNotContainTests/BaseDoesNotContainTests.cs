using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Xenon.Tests.Integration;

namespace Xenon.Tests.DoesNotContainTests
{
	public abstract class BaseDoesNotContainTests<T> : BaseXenonIntegrationTest where T : BaseXenonTest<T>
	{
		protected abstract BaseXenonTest<T> CreateInstance(IXenonBrowser browser);

		protected BaseDoesNotContainTests()
		{
			XenonTestsResourceLookup.Folder( "DoesNotContainTests" );
		}

		[Test]
		public void DoesNotContainCssSelector_ReturnsTrueWhenDoesNotContain()
		{
			var html = XenonTestsResourceLookup.GetContent( "DoesNotContain" );
			using ( var browserTest = new BrowserTest( html ) )
			{
				var browser = browserTest.Start();
				CreateInstance( browser )
					.Assert( x => x.DoesNotContainElement( ".not-there" ) );
			}
		}
	}
}