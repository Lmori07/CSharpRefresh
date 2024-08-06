using Microsoft.Playwright;

namespace CSharpRefresh.Pages;

public class LoginPage{
    private IPage _page;
    //The region will contain the variable created for the locator element of the login page.
    #region Loing Page Locators
    private readonly ILocator _lnkLogin;
    private readonly ILocator _txtUsernName;
    private readonly ILocator _txtPassword;
    private readonly ILocator _btnLogin;
    private readonly ILocator _lnkEmployeeDetails;
    #endregion

    //Constructor of LoginPage where all variable will get the corresponding xpath.
    public LoginPage(IPage Page){
    _page = Page;
    _lnkLogin = _page.Locator("text=Login");
    _txtUsernName = _page.Locator("#UserName");
    _txtPassword = _page.Locator("#Password");
    _btnLogin = _page.Locator("input", new PageLocatorOptions {HasTextString = "Log in"});
    _lnkEmployeeDetails = _page.Locator("text=Employee Details");
    }   

    public async Task ClickLogin(){
        await _lnkLogin.ClickAsync();
    }

    public async Task Login(string userName, string password){
        await _txtUsernName.FillAsync(userName);
        await _txtPassword.FillAsync(password);
        await _btnLogin.ClickAsync();
    }

    public async Task<bool> IsEmployeeDetailExists() => await _lnkEmployeeDetails.IsVisibleAsync();
}