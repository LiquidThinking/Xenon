using System;

namespace Xenon
{
	public class XenonScreen<T> : BaseXenonTest<T> where T : XenonScreen<T>
	{
		public XenonScreen( IXenonBrowser browser, XenonTestOptions options ) : base( browser, options ) { }

		public TNew Switch<TNew>() where TNew : XenonScreen<TNew>
		{
			try
			{
				return (TNew)Activator.CreateInstance( typeof( TNew ), _xenonBrowser, _xenonTestOptions );
			}
			catch
			{
				throw new Exception( $"Cannot find a constructor with parameter of type ({nameof(IXenonBrowser)}) or ({nameof(IXenonBrowser)}, {nameof(XenonTestOptions)}) " );
			}
		}
	}
}