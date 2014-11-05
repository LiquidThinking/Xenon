using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

namespace Xenon.Tests.Intergration
{

	public class HttpProcessor
	{
		public TcpClient Socket;
		public HttpServer Srv;

		private Stream _inputStream;
		public StreamWriter OutputStream;

		public String HttpMethod;
		public String HttpUrl;
		public String HttpProtocolVersionstring;
		public Hashtable HttpHeaders = new Hashtable();


	    private const int MaxPostSize = 10*1024*1024; // 10MB

	    public HttpProcessor( TcpClient s, HttpServer srv )
		{
			Socket = s;
			Srv = srv;
		}


		private string StreamReadLine( Stream inputStream )
		{
		    var data = "";
			while ( true )
			{
				var nextChar = inputStream.ReadByte();
				if ( nextChar == '\n' ) { break; }
				if ( nextChar == '\r' ) { continue; }
				if ( nextChar == -1 ) { Thread.Sleep( 1 ); continue; };
				data += Convert.ToChar( nextChar );
			}
			return data;
		}
		public void Process()
		{
			// we can't use a StreamReader for input, because it buffers up extra data on us inside it's
			// "processed" view of the world, and we want the data raw after the headers
			_inputStream = new BufferedStream( Socket.GetStream() );

			// we probably shouldn't be using a streamwriter for all output from handlers either
			OutputStream = new StreamWriter( new BufferedStream( Socket.GetStream() ) );
			try
			{
				ParseRequest();
				ReadHeaders();
				if ( HttpMethod.Equals( "GET" ) )
				{
					HandleGetRequest();
				}
				else if ( HttpMethod.Equals( "POST" ) )
				{
					HandlePostRequest();
				}
			}
			catch (Exception e)
			{
				Console.WriteLine( "Exception: " + e.ToString() );
				WriteFailure();
			}
			OutputStream.Flush();
			// bs.Flush(); // flush any remaining output
			_inputStream = null; OutputStream = null; // bs = null;            
			Socket.Close();
		}

		public void ParseRequest()
		{
			var request = StreamReadLine( _inputStream );
			var tokens = request.Split( ' ' );
			if ( tokens.Length != 3 )
			{
				throw new Exception( "invalid http request line" );
			}
			HttpMethod = tokens[ 0 ].ToUpper();
			HttpUrl = tokens[ 1 ];
			HttpProtocolVersionstring = tokens[ 2 ];

			Console.WriteLine( "starting: " + request );
		}

		public void ReadHeaders()
		{
			Console.WriteLine( "readHeaders()" );
			String line;
			while ( ( line = StreamReadLine( _inputStream ) ) != null )
			{
				if ( line.Equals( "" ) )
				{
					Console.WriteLine( "got headers" );
					return;
				}

				var separator = line.IndexOf( ':' );
				if ( separator == -1 )
				{
					throw new Exception( "invalid http header line: " + line );
				}
				var name = line.Substring( 0, separator );
				var pos = separator + 1;
				while ( ( pos < line.Length ) && ( line[ pos ] == ' ' ) )
				{
					pos++;   // strip any spaces
				}

				var value = line.Substring( pos, line.Length - pos );
				Console.WriteLine( "header: {0}:{1}", name, value );
				HttpHeaders[ name ] = value;
			}
		}

		public void HandleGetRequest()
		{
			Srv.HandleGetRequest( this );
		}

		private const int BufSize = 4096;
		public void HandlePostRequest()
		{
			// this post data processing just reads everything into a memory stream.
			// this is fine for smallish things, but for large stuff we should really
			// hand an input stream to the request processor. However, the input stream 
			// we hand him needs to let him see the "end of the stream" at this content 
			// length, because otherwise he won't know when he's seen it all! 

			Console.WriteLine( "get post data start" );
			var contentLen = 0;
			var ms = new MemoryStream();
			if ( HttpHeaders.ContainsKey( "Content-Length" ) )
			{
				contentLen = Convert.ToInt32( HttpHeaders[ "Content-Length" ] );
				if ( contentLen > MaxPostSize )
				{
					throw new Exception(
						String.Format( "POST Content-Length({0}) too big for this simple server",
						  contentLen ) );
				}
				var buf = new byte[ BufSize ];
				var toRead = contentLen;
				while ( toRead > 0 )
				{
					Console.WriteLine( "starting Read, to_read={0}", toRead );

					var numread = _inputStream.Read( buf, 0, Math.Min( BufSize, toRead ) );
					Console.WriteLine( "read finished, numread={0}", numread );
					if ( numread == 0 )
					{
						if ( toRead == 0 )
						{
							break;
						}
						else
						{
							throw new Exception( "client disconnected during post" );
						}
					}
					toRead -= numread;
					ms.Write( buf, 0, numread );
				}
				ms.Seek( 0, SeekOrigin.Begin );
			}
			Console.WriteLine( "get post data end" );
			Srv.HandlePostRequest( this, new StreamReader( ms ) );

		}

		public void WriteSuccess( string contentType = "text/html" )
		{
			OutputStream.WriteLine( "HTTP/1.0 200 OK" );
			OutputStream.WriteLine( "Content-Type: " + contentType );
			OutputStream.WriteLine( "Connection: close" );
			OutputStream.WriteLine( "" );
		}

		public void WriteFailure()
		{
			OutputStream.WriteLine( "HTTP/1.0 404 File not found" );
			OutputStream.WriteLine( "Connection: close" );
			OutputStream.WriteLine( "" );
		}
	}

	public abstract class HttpServer : IDisposable
	{

		protected int Port;
		TcpListener _listener;
	    bool _isActive = true;

		public HttpServer( int port )
		{
			Port = port;
		}

		public void Listen()
		{
			_listener = new TcpListener( Port );
			_listener.Start();
			while ( _isActive )
			{
				var s = _listener.AcceptTcpClient();
				var processor = new HttpProcessor( s, this );
				var thread = new Thread( new ThreadStart( processor.Process ) );
				thread.Start();
				Thread.Sleep( 1 );
			}
		}

		public abstract void HandleGetRequest( HttpProcessor p );
		public abstract void HandlePostRequest( HttpProcessor p, StreamReader inputData );
	    public void Dispose()
	    {
	        _isActive = false;
	    }

        public abstract NameValueCollection GetPostResult();
	}

	public class MyHttpServer : HttpServer
	{
	    private readonly Page _page;

	    private NameValueCollection _postResult;

		public MyHttpServer( int port, Page page )
			: base( port )
		{
		    _page = page;
		}
		public override void HandleGetRequest( HttpProcessor p )
		{
		        foreach (var line in Regex.Split(_page.Html, @"\r?\n|\r"))
		        {
                    p.OutputStream.WriteLine(line);
		        }
		}

		public override void HandlePostRequest( HttpProcessor p, StreamReader inputData )
		{
			Console.WriteLine( "POST request: {0}", p.HttpUrl );
			var data = inputData.ReadToEnd();

		    _postResult = HttpUtility.ParseQueryString(data);

			p.WriteSuccess();
			p.OutputStream.WriteLine( "<html><body><h1>test server</h1>" );
			p.OutputStream.WriteLine( "<a href=/test>return</a><p>" );
			p.OutputStream.WriteLine( "postbody: <pre>{0}</pre>", data );


		}

	    public override NameValueCollection GetPostResult()
	    {
	        var startTime = DateTime.Now;
	        while (startTime.AddMinutes(2) > DateTime.Now)
	        {
	            if (_postResult != null)
	                return _postResult;
	        }
	        return null;
	    }
	}

}



