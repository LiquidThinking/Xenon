namespace Xenon
{
	public static class XenonBrowserExtensions
	{
		/// <summary>
		/// Refresh the page by going to the Browser's current Url
		/// </summary>
		/// <typeparam name="TScreen"></typeparam>
		/// <param name="screen"></param>
		/// <param name="waitForCssSelector"></param>
		/// <returns></returns>
		public static TScreen RefreshPage<TScreen>( this TScreen screen, string waitForCssSelector = null )
			where TScreen : BaseXenonTest<TScreen>
		{
			return screen
				.Custom(
					browser => browser.GoToUrl( browser.Url ),
					customPreWait: null,
					WaitForSelector );

			XenonAssertion WaitForSelector( XenonAssertion assertion )
			{
				return string.IsNullOrEmpty( waitForCssSelector )
					? assertion
					: assertion.ContainsElement( waitForCssSelector );
			}
		}

		/// <summary>
		/// Mimic clicking 'back' in the browser
		/// </summary>
		/// <typeparam name="TFrom"></typeparam>
		/// <typeparam name="TTo"></typeparam>
		/// <param name="screen"></param>
		/// <param name="postWait"></param>
		/// <returns></returns>
		public static TTo Back<TFrom, TTo>( this TFrom screen, AssertionFunc postWait = null )
			where TFrom : XenonScreen<TFrom>
			where TTo : XenonScreen<TTo>
		{
			return screen
				.Custom(
					browser => browser
						.ExecuteJavascript( "window.history.back();" ),
					customPreWait: null,
					customPostWait: postWait )
				.Switch<TTo>();
		}
	}
}