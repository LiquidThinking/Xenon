using System;
using System.Collections.Specialized;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using Owin;

namespace Xenon.Tests.Integration
{
	public class Startup
	{
		public static string Html { get; set; }
		public static NameValueCollection PostbackResult { get; set; }
		public void Configuration( IAppBuilder app )
		{
			app.Use( (context,a) =>
			{

				if ( context.Request.Path.Value == "/Google" )
				{
					context.Response.ContentType = "text/html";
					return context.Response.WriteAsync( "google" );
				}

				if ( context.Request.Path.Value == "/Github" )
				{
					context.Response.ContentType = "text/html";
					return context.Response.WriteAsync( "github" );
				}

				if ( context.Request.Path.Value == "/Yahoo" )
				{
					context.Response.ContentType = "text/html";
					return context.Response.WriteAsync( "yahoo" );
				}



				if ( context.Request.Method == "GET" )
				{
					context.Response.ContentType = "text/html";
					return context.Response.WriteAsync( Html );
				}

				else if ( context.Request.Method == "POST" )
				{
					var formAsync = context.Request.ReadFormAsync();
					formAsync.Wait();


					PostbackResult = new NameValueCollection();
					foreach ( var val in formAsync.Result )
					{
						PostbackResult.Add( val.Key, String.Join(",",val.Value) );
					}
                }
				return Task.Delay( 0 );

			} );
		}

		public static NameValueCollection GetPostResult()
		{
			var startTime = DateTime.Now;
			while ( startTime.AddMinutes( 2 ) > DateTime.Now )
			{
				if ( PostbackResult != null )
					return PostbackResult;
			}
			return null;
		}
	}
}