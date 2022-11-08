using System;
using BLKAutoFramework.Base;
using BLKAutoFramework.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BLKSupportPortalDemo.Pages
{
    internal class ProvisioningPge : BasePage
    {
        public ProvisioningPge(ParallelConfig parallelConfig) : base(parallelConfig)
        {

        }
        IWebElement? DeviceexplorerLinkBtn => _parallelConfig.Driver!.FindByXpath("//*[contains(text(),'Device Explorer')]");
        IWebElement? EnrollmentGrpLinkBtn => _parallelConfig.Driver!.FindByXpath("//*[contains(text(),'Enrollment Group')]");
        IWebElement? ProvisioningDropdown => _parallelConfig.Driver!.FindByXpath("//select[@data-testid='provisioningStatus']");
        SelectElement? GroupNameDropdown => _parallelConfig.Driver!.FindSelectElementWhenPopulated("//select[@data-testid='enrolmentGroup']");
        IWebElement? SearchBtn => _parallelConfig.Driver!.FindByXpath("//button[contains(text(),'Search')]");
        IWebElement? UsergroupBtn => _parallelConfig.Driver!.FindByCss("div.btn-group");
        IWebElement? LogoutBtn => _parallelConfig.Driver!.FindByXpath("//span[contains(text(),'Logout')]");

        internal void CheckIfNavigatedtoProvisioning()
        {
            DeviceexplorerLinkBtn!.AssertElementPresent();
            EnrollmentGrpLinkBtn!.AssertElementPresent();
        }
        internal void SelectProvisioningStatus(string provStatus)
        {
            ProvisioningDropdown!.SelectDropDownList(provStatus);
        }
        internal void SelectGroupName(string grpName)
        {
            GroupNameDropdown!.SelectByValue(grpName);
        }
        internal void ClickonSearch()
        {
            SearchBtn!.Click();
        }
        public LoginPage ClickLogOff()
        {
            UsergroupBtn!.Click();
            LogoutBtn!.Click();
            return new LoginPage(_parallelConfig);
        }
    }
}
