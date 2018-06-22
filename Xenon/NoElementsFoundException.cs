using System;

namespace Xenon
{
	/// <summary>
	/// Thrown when search criteria return no results
	/// </summary>
	public class NoElementsFoundException : Exception
	{
		public NoElementsFoundException( string message )
			: base( message )
		{
		}
	}
}