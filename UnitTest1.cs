using System.Web;
using CSharpRefresh.Pages;
using FluentAssertions;
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
    [Test]
     public async Task BasicsULRWithPOM()
    {
        /*
        To run it headed mode use the following command:
        $env:HEADED="1"
        dotnet test
        */
        /*This is to use the classic POM format.*/
        LoginPage loginPage = new LoginPage(Page);

        /*This is to use the upgraded version of the POM format.*/
        LoginPageUpgrade loginPageUpgrade = new LoginPageUpgrade(Page);

        await loginPageUpgrade.ClickLogin();
        await loginPageUpgrade.Login("admin", "password");
        var isExist = await loginPageUpgrade.IsEmployeeDetailExists();
        Assert.That(isExist, Is.True);

    }
    [Test]
     public async Task ValidatingNetworkResponse()
    {
        /*
        To run it headed mode use the following command:
        $env:HEADED="1"
        dotnet test
        */
        /*This is to use the classic POM format.*/
        LoginPage loginPage = new LoginPage(Page);

        /*This is to use the upgraded version of the POM format.*/
        LoginPageUpgrade loginPageUpgrade = new LoginPageUpgrade(Page);
        await loginPageUpgrade.ClickLogin();
        await loginPageUpgrade.Login("admin", "password");

        /*Here we run and wait for the ResponseAsync to load all the data on response variable
         when it clicks on the Employee list, After that will validate that the URL contains the word Employee 
         and status code is 200.*/
        var response = await Page.RunAndWaitForResponseAsync(async () =>{
            await loginPageUpgrade.ClickEmployeeList();
        }, x => x.Url.Contains("/Employee") && x.Status == 200);

        var isExist = await loginPageUpgrade.IsEmployeeDetailExists();
        Assert.That(isExist, Is.True);

    }

    [Test]
     public async Task Flipkart()
    {
        /*
        To run it headed mode use the following command:
        $env:HEADED="1"
        dotnet test
        */
        //Specifying that i want to see the browser
        var browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions{ 
            Headless = false 
        });

        // Create a new incognito browser context with empty cookies.
        var context = await browser.NewContextAsync();

        // Create a new page inside context.
        var page = await context.NewPageAsync();

        //We are navigating to flipkart and waiting until state is "NetworkIdle"
        await page.GotoAsync("https://flipkart.com/", new PageGotoOptions{
            WaitUntil = WaitUntilState.NetworkIdle
        });

        //Locator for the Login Button.
        //await page.Locator("//span[normalize-space()='Login']").ClickAsync();

        //This is because the login page opens right away and we need to close it to capture the information from the netwrok tab.
        await page.Locator("text=x").ClickAsync();

        //Here i locate an anchor element that has a Text String value of "Login" and click it
         await page.Locator("a", new PageLocatorOptions{
            HasTextString = "Login"
        }).ClickAsync();

        // Here we wait to close the login modal and get all the network information from the request
        var request = await page.RunAndWaitForRequestAsync( async() =>{
            await page.Locator("text=x").ClickAsync();
        }, x => x.Url.Contains("flipkart.d1.sc.omtrdc.net") && x.Method == "GET");

        //This will decode the URL to get the actual URL
        var returnData = HttpUtility.UrlDecode(request.Url);

        //Here we validate that the URL contain the following information letting us know what happened on the network side.
        returnData.Should().Contain("Account Login:Displayed");

        // Dispose context once it is no longer needed.
        await context.CloseAsync();


    }

    [Test]
     public async Task FlipkartNetworkInterception(){
        var browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions{ 
            Headless = false 
        });

        // Create a new incognito browser context with empty cookies.
        var context = await browser.NewContextAsync();

        // Create a new page inside context.
        var page = await context.NewPageAsync();

        //We are implementing delegate code from C# to writhe in console all the URL method happening when trying to access flipkart, NOTE: delage short route is "+=" happening in the request.
        page.Request += (_, request) => Console.WriteLine(request.Method + "---" + request.Url);

        //We are implementing delegate code from C# to writhe in console all the status response happening when trying to access flipkart, NOTE: delage short route is "+=" happening in the response.
        page.Response += (_, response) => Console.WriteLine(response.Status + "---" + response.Url);

        // Here we listen to network events and abort all image requests, in other hand "**/*" means that URL is been affected.
        await page.RouteAsync("**/*", async route =>{
            if(route.Request.ResourceType == "image"){
                await route.AbortAsync();
            }else
            {
                await route.ContinueAsync();
            }
        });

        //We are navigating to flipkart and waiting until state is "NetworkIdle"
        await page.GotoAsync("https://flipkart.com/", new PageGotoOptions{
            WaitUntil = WaitUntilState.NetworkIdle
        });
     }
}