using System.Reflection.Metadata.Ecma335;
using Microsoft.Playwright;

namespace CSharpRefresh.Pages;

public class LoginPageUpgrade{
    private IPage _page;
    public LoginPageUpgrade(IPage Page) => _page = Page;

    //The region will contain the variable created for the locator element of the login page.
    #region Loing Page Locators
    private ILocator _lnkLogin => _page.Locator("text=Login");
    private ILocator _txtUsernName => _page.Locator("#UserName");
    private ILocator _txtPassword => _page.Locator("#Password");
    private ILocator _btnLogin => _page.Locator("input", new PageLocatorOptions {HasTextString = "Log in"});
    private ILocator _lnkEmployeeDetails => _page.Locator("text=Employee Details");
     private ILocator _lnkEmployeeLink => _page.Locator("text=Employee Details");
    #endregion

    public async Task ClickLogin(){
        await _lnkLogin.ClickAsync();
    }

    public async Task ClickEmployeeList() => await _lnkEmployeeLink.ClickAsync();

    public async Task Login(string userName, string password){
        await _txtUsernName.FillAsync(userName);
        await _txtPassword.FillAsync(password);
        await _btnLogin.ClickAsync();
    }

    public async Task<bool> IsEmployeeDetailExists() => await _lnkEmployeeDetails.IsVisibleAsync();
}