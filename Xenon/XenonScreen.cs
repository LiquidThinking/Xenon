using System;
using System.Linq;
using System.Reflection;

namespace Xenon
{
	public class XenonScreen<T> : BaseXenonTest<T> where T : XenonScreen<T>
	{
		public XenonScreen( IXenonBrowser xenonBrowser ) : base( xenonBrowser ) { }
		public XenonScreen( IXenonBrowser browser, XenonTestOptions options ) : base( browser, options ) { }

		private static ConstructorInfo ConstructorContainingIXenonBrowserAndXenonTestOptions<TNew>( ConstructorInfo[] constructors ) where TNew : XenonScreen<TNew>
		{
			return constructors.FirstOrDefault( x => x.GetParameters().Count() == 2 && x.GetParameters().First().ParameterType == typeof( IXenonBrowser ) && x.GetParameters().ElementAt( 1 ).ParameterType == typeof( XenonTestOptions ) );
		}

		private static ConstructorInfo ConstructorContainingIXenonBrowser<TNew>( ConstructorInfo[] constructors ) where TNew : XenonScreen<TNew>
		{
			return constructors.FirstOrDefault(x => x.GetParameters().Count() == 1 && x.GetParameters().First().ParameterType == typeof(IXenonBrowser)   );
		}

		private TNew CreateInstance<TNew>( ConstructorInfo constructor, params object[] parameters ) where TNew : XenonScreen<TNew>
		{
			return (TNew)constructor.Invoke( parameters );
		}

		public TNew Switch<TNew>() where TNew : XenonScreen<TNew>
		{
			var constructors = typeof( TNew ).GetConstructors();

			var constructorWith2Params = ConstructorContainingIXenonBrowserAndXenonTestOptions<TNew>( constructors );
			if ( constructorWith2Params != null )
				return CreateInstance<TNew>( constructorWith2Params, _xenonBrowser, _xenonTestOptions);

			var constructorWith1Param = ConstructorContainingIXenonBrowser<TNew>( constructors );
			if ( constructorWith1Param != null )
				return CreateInstance<TNew>( constructorWith1Param, _xenonBrowser );

			throw new Exception("Cannot find a constructor with parameter of type (IXenonBrowser) or (IXenonBrowser, XenonTestOptions) ");
		}
	}
}