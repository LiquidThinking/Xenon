namespace Xenon
{
	public interface IXenonElement
	{
		IXenonElement Click();
		IXenonElement EnterText( string value );
		bool IsVisible { get; }
	    string Text { get; }
		IXenonElement Clear();
		IXenonElement ScrollToElement();
	}
}
