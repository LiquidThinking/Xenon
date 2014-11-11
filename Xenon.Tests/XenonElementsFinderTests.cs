using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Xenon.Tests.ExtensionMethods;

namespace Xenon.Tests
{
	[TestFixture]
	public class XenonElementsFinderTests : BaseXenonTest
	{
		[Test]
		public void AttributeIs_ElementPresent_ReturnsThatElement()
		{
			var browser = SetupBrowser();
			browser.SetupFindElementsByXPath( It.IsAny<string>() );
		}
	}
}
