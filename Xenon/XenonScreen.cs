using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text;

namespace Xenon
{
	public class XenonScreen<T> : BaseXenonTest<T> where T : XenonScreen<T>
	{
		public TNew Switch<TNew>() where TNew : XenonScreen<TNew>
		{
			return (TNew)typeof(TNew)
				.GetConstructor( new[ ] { typeof(IXenonBrowser), typeof(XenonTestOptions) } )
				.Invoke( new object[] { _xenonBrowser, _xenonTestOptions } );
		}

		public XenonScreen( IXenonBrowser xenonBrowser ) : base( xenonBrowser ) {}
		public XenonScreen( IXenonBrowser browser, XenonTestOptions options ) : base( browser, options ) {}
	}
}