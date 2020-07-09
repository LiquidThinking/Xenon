using System;

namespace Xenon
{
	public class XenonTestOptions
	{
		private Action<bool, string> _assertMethod;
		public int WaitForSeconds { get; set; }

		/// <summary>
		/// Optional hook to validate pages _generally_ after explicit navigation & in assertions
		/// I.e. Check for default web server error text, or a malformed Url
		/// </summary>
		public Func<Page, string> PageValidationFunc { get; set; }

		public XenonTestOptions()
		{
			WaitForSeconds = 5;
		}

		public Action<bool, string> AssertMethod
		{
			get
			{
				if ( _assertMethod == null )
					throw new Exception( "You must set an AssertMethod" );
				return _assertMethod;
			}
			set { _assertMethod = value; }
		}

		public static XenonTestOptions Options { get; set; }
	}

	public class Page
	{
		public string Url { get; }
		public string Source { get; }

		public Page( string url, string source )
		{
			Url = url;
			Source = source;
		}
	}
}