using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xenon
{
	public interface IXenonElement
	{
		void Click();
		void SetValue( string value );
	}
}
