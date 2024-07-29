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
    public async Task basicsULROpenning()
    {
        await Page.ClickAsync("text='Login'");

        await Page.FillAsync("#UserName", "admin");
        await Page.FillAsync("#Password", "password");
        await Page.ClickAsync("text= Log in");
        await Expect(Page.Locator("text='Employee Details'")).ToBeVisibleAsync();
    }
}