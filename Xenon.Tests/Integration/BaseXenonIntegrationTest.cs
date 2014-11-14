using System;
using System.IO;
using System.Reflection;

namespace Xenon.Tests.Integration
{
	public class BaseXenonIntegrationTest
	{
		protected EmbeddedResourceLookup XenonTestsResourceLookup { get; set; }


		public BaseXenonIntegrationTest()
		{
			XenonTestsResourceLookup = new EmbeddedResourceLookup()
				.Folder( "Xenon" )
				.Folder( "Tests" );
		}
	}
}