using BLKAutoFramework.Base;
using BLKAutoFramework.Extensions;
using OpenQA.Selenium;

namespace BLKSupportPortalDemo.Pages
{
    class LoginPage : BasePage
    {
        public LoginPage(ParallelConfig parallelConfig) : base(parallelConfig) { }
        IWebElement? TxtUserName => _parallelConfig.Driver!.FindByXpath("//input[@name='loginfmt']");
        IWebElement? NextBtn => _parallelConfig.Driver!.FindByXpath("//input[@value='Next']");
        IWebElement? TxtPassword => _parallelConfig.Driver!.FindByXpath("//input[@name='passwd']");
        IWebElement? SigninBtn => _parallelConfig.Driver!.FindByXpath("//input[@value='Sign in']");
        IWebElement? ConfirmBtn => _parallelConfig.Driver!.FindByXpath("//input[@value='Yes']");

        public void Login(string userName, string password)
        {
            TxtUserName!.SendKeys(userName);
            NextBtn!.Click();
            TxtPassword!.SendKeys(password);
            SigninBtn!.Click();
        }
        public HomePage ClickLoginButton()
        {
            ConfirmBtn!.Submit();
            return new HomePage(_parallelConfig);
        }
        internal void CheckIfLoginExist()
        {
            TxtUserName!.AssertElementPresent();
        }
    }
}
