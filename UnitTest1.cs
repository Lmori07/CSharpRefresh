using Microsoft.Playwright;

namespace CSharpRefresh;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task basicsULROpenning()
    {
        //Playwright
        using var playwright = await Playwright.CreateAsync();
        //Browser
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false
        });
        //Page
        var page = await browser.NewPageAsync();
        await page.GotoAsync("http://www.eaapp.somee.com/");
        await page.ClickAsync("text='Login'");
        await page.ScreenshotAsync(new PageScreenshotOptions 
        {
            Path = "EAAPP.jpg"
        });
    }
}