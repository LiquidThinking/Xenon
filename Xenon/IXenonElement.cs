using System;

namespace Xenon
{
	public interface IXenonElement
	{
		IXenonElement Click();
		IXenonElement RightClick();
		IXenonElement EnterText( string value );
		IXenonElement SelectDropdownItem( string name );
		bool IsVisible { get; }
	    string Text { get; }
		IXenonElement Clear();
		IXenonElement ScrollToElement();
		IXenonElement EnterDate( DateTime date );
	}
}
