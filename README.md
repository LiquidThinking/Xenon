Xenon
=====

Packages
-------------
- [Xenon](https://www.nuget.org/packages/Xenon)
- [Xenon.Selenium](https://www.nuget.org/packages/Xenon.Selenium)

Documentation
-------------
Xenon is a test framework which lets you to write stable acceptance tests in a fluent api manner.

    // if you are using nunit then 
    // this tells xenon what method to call when you do XenonTest.Assert, only needs to be set once
    XenonTestOptions.Options.AssertMethod = Assert.IsTrue;
    
    //In your test
    new XenonTest(new SeleniumXenonBrowser())
            .GoToUrl("http://www.google.co.uk")
            .EnterText("input[name='q']", "xenon test framework")
            .Click("button[name='btnK']")
            .Assert( a => a.PageContains("results") );

If you have and issues or this the api should be different please raise an issue.
