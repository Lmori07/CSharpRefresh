using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace CSharpRefresh;

public class Tests : PageTest
{
    [SetUp]
    public async Task Setup()
    {
        await Page.GotoAsync("http://www.eaapp.somee.com/");
    }

    [Test]
    public async Task BasicsULROpening()
    {
        /*
        To run it headed mode use the following command:
        $env:HEADED="1"
        dotnet test
        */

        #region Locators
        var usernameTextBox = Page.Locator("#UserName");
        var passwordTextBox = Page.Locator("#Password");
        var loginButton = Page.Locator("input", new PageLocatorOptions {HasTextString = "Log in"}); //In here we are setting the locator to identify an input with a value string of "Log in"
        #endregion
        
        await Page.ClickAsync("text='Login'");
        await usernameTextBox.FillAsync("admin");
        await passwordTextBox.FillAsync("password");
        await loginButton.ClickAsync();
        await Expect(Page.Locator("text='Employee Details'")).ToBeVisibleAsync();
    }
}