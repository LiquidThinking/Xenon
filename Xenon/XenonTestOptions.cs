using System;

namespace Xenon
{
	public class XenonTestOptions
	{
		public const string DefaultDateFormat = "yyyy-MM-dd";
		private Action<bool, string> _assertMethod;
		public int WaitForSeconds { get; set; }

		/// <summary>
		/// Optional hook to validate pages _generally_ after explicit navigation & in assertions
		/// I.e. Check for default web server error text, or a malformed Url
		/// </summary>
		public Validation Validation { get; set; }

		/// <summary>
		/// The format to use for <see cref="DateTime"/>, defaults to <see cref="DefaultDateFormat"/>
		/// </summary>
		public string DateFormat { get; set; }

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
			set
			{
				_assertMethod = value;
			}
		}

		public static XenonTestOptions Options { get; set; }

		/// <summary>
		/// Clone this instance but change whatever properties you need to
		/// </summary>
		/// <param name="mutate"></param>
		/// <returns></returns>
		public XenonTestOptions Clone( Action<XenonTestOptions> mutate )
		{
			var @new = new XenonTestOptions
			{
				WaitForSeconds = WaitForSeconds,
				AssertMethod = AssertMethod,
				Validation = Validation,
				DateFormat = DateFormat
			};

			mutate( @new );
			return @new;
		}
	}

	public class Validation
	{
		public NavAction NavAction { get; set; }
		public Func<Page, string> Func { get; set; }

		public Validation Clone( Action<Validation> mutate )
		{
			var @new = new Validation
			{
				Func = Func,
				NavAction = NavAction
			};

			mutate( @new );
			return @new;
		}
	}

	[Flags]
	public enum NavAction
	{
		None = 0,
		GoToUrl = 1,
		Click = 2,
		Custom = 4
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