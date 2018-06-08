using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xenon
{
	public class IncorrectInputElementTypeException : Exception
	{
		public IncorrectInputElementTypeException( string message )
			: base( message )
		{
		}
	}
}