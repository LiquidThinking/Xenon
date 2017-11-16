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
		[OneTimeSetUp]
		public void Initialize()
		{
			XenonTestOptions.Options = new XenonTestOptions
			{
				AssertMethod = ( b, s ) => Assert.IsTrue( b, s ),
			};
		}
	}
}