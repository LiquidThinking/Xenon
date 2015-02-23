using System.IO;
using System.Reflection;
using System.Text;

namespace Xenon.Tests
{
	public class EmbeddedResourceLookup
	{
		private readonly StringBuilder _pathBuilder;

		public EmbeddedResourceLookup()
		{
			_pathBuilder = new StringBuilder();
		}

		public EmbeddedResourceLookup Folder( string name )
		{
			_pathBuilder.Append( name + "." );
			return this;
		}

		private string GetFullPath( string fileName )
		{
			return _pathBuilder + fileName + ".html";
		}

		public string GetContent( string htmlFileName )
		{
			string resourceIdentifier = GetFullPath( htmlFileName );
			var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream( resourceIdentifier );
			var content = new StreamReader( stream ).ReadToEnd();
			return content;
		}
	}
}