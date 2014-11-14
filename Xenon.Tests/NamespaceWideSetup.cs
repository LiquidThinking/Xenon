using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Xenon.Tests
{
	[SetUpFixture]
	public class NamespaceWideSetup
	{
		[SetUp]
		public void Initialize()
		{
			XenonTestOptions.Options = new XenonTestOptions
			{
				AssertMethod = Assert.IsTrue,
			};
		}
	}
}
