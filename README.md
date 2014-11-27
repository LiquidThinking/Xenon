

![](http://i60.tinypic.com/148er6d.png) Xenon <a href="http://buildserver.spawtz.com:8000/viewType.html?buildTypeId=Xenon_Build&guest=1"><img src="http://buildserver.spawtz.com:8000/app/rest/builds/buildType:(id:Xenon_Build)/statusIcon"/></a>
=====
[![Gitter](https://badges.gitter.im/Join Chat.svg)](https://gitter.im/LiquidThinking/Xenon?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

Packages
-------------
- [Xenon](https://www.nuget.org/packages/Xenon)
- [Xenon.Selenium](https://www.nuget.org/packages/Xenon.Selenium)

###Visit the [wiki](https://github.com/LiquidThinking/Xenon/wiki) for more details.


Documentation
-------------
Xenon is a test framework which lets you to write stable acceptance tests in a fluent api manner.

    // if you are using nunit then 
    // this tells xenon what method to call when you do XenonTest.Assert, only needs to be set once
	XenonTestOptions.Options = new XenonTestOptions
	{
		AssertMethod = Assert.IsTrue
	};
			
    //In your test
    new XenonTest(new SeleniumXenonBrowser())
            .GoToUrl("http://www.google.co.uk")
            .EnterText("input[name='q']", "xenon test framework")
            .Click("button[name='btnK']")
            .Assert( a => a.PageContains("results") );
            
How To Use
---------------

There are two ways to use Xenon in your acceptance test. You can either directly create a instance of XenonTest as in the previous example or you can create a class inheriting from XenonScreen. So if we redo the previous example using XenonScreen then this is how we do it

	public class GoogleHomeScreen : XenonScreen<GoogleHomePage>
	{
		public GoogleHomeScreen(IXenonBrowser browser) : base(browser)
		{
			GotoUrl("http://www.google.com");
		}
	
		public GoogleSearchResultsScreen Search(string text)
		{
			return EnterText("input[name='q']", text)
			       .Click("button[name='btnK']")
			       .Switch<GoogleSearchResultsScreen>();
		}
	}
	
	public class GoogleSearchResultsScreen : XenonScreen<GoogleSearchResultsScreen>
	{
		public GoogleSearchResultsScreen(IXenonBrowser browser) : base(browser) { }
	}
	
	[Test]
	public void CanVisitGoogle()
	{
		new GoogleHomeScreen(new SeleniumXenonBrowser(driver))
		.Search("xenon test framework")
		.Assert( a => a.PageContains("results") );
	}
