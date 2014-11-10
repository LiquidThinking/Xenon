using System;
using System.Collections.Specialized;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using Owin;

namespace Xenon.Tests.Intergration
{
	public class Startup
	{
		public static string Html { get; set; }
		public static NameValueCollection PostbackResult { get; set; }
		public void Configuration( IAppBuilder app )
		{
			app.Use( (context,a) =>
			{
				
				if ( context.Request.Method == "GET" )
				{
					context.Response.ContentType = "text/html";
					return context.Response.WriteAsync( Html );
				}

				else if ( context.Request.Method == "POST" )
				{
					Stream req = context.Request.Body;
					req.Seek( 0, System.IO.SeekOrigin.Begin );
					string body = new StreamReader( req ).ReadToEnd();

					PostbackResult = HttpUtility.ParseQueryString( body );
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