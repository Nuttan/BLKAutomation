using System;
using BLKAutoFramework.Base;
using BLKAutoFramework.Extensions;
using OpenQA.Selenium;

namespace BLKSupportPortalDemo.Pages
{
    internal class HomePage : BasePage
    {
        public HomePage(ParallelConfig parallelConfig) : base(parallelConfig)
        {

        }
        IWebElement? LnkLogin => _parallelConfig.Driver!.FindElementByClassName("btn-login");
        IWebElement? ProvisioningLink => _parallelConfig.Driver!.FindByXpath("//label[contains(text(),'Provisioning')]");
        IWebElement? HomeTxtlabel => _parallelConfig.Driver!.FindByXpath("//a[@data-testid='home']");

        internal void CheckIfLoginExist()
        {
            LnkLogin!.AssertElementPresent();
        }
        internal LoginPage ClickLogin()
        {
            LnkLogin!.Click();
            return new LoginPage(_parallelConfig);
        }
        internal string GetLoggedInUser()
        {
            return HomeTxtlabel!.Text;
        }
        public ProvisioningPge ClickProvisioning()
        {
            ProvisioningLink!.Click();
            return new ProvisioningPge(_parallelConfig);
        }
    }
}
