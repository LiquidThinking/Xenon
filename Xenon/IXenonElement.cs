namespace Xenon
{
	public interface IXenonElement
	{
		void Click();
		void EnterText( string value );
		bool IsVisible { get; }
	    string Text { get; }
	}
}
