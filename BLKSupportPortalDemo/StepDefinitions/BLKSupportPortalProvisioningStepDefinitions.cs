using BLKAutoFramework.Base;
using BLKAutoFramework.Utility;
using BLKSupportPortalDemo.Pages;
using TechTalk.SpecFlow.Assist;

namespace BLKSupportPortalDemo
{
    [Binding]
    public class BlkSupportPortalProvisioningStepDefinitions : BaseStep
    {
        private readonly IFeaturesConfiguration _featuresConfiguration;

        //Context injection
        private readonly ParallelConfig _parallelConfig;
        public BlkSupportPortalProvisioningStepDefinitions(ParallelConfig parallelConfig,IFeaturesConfiguration featuresConfiguration) : base(parallelConfig)
        {
            _featuresConfiguration = featuresConfiguration;
            _parallelConfig = parallelConfig;
        }
        private void NavigateSite()
        {
            _parallelConfig.Driver!.Navigate().GoToUrl(_featuresConfiguration.ApplicationURL);
            _parallelConfig.Driver!.Manage().Window.Maximize();
        }

        [Given(@"I have navigated to the application")]
        public void GivenIHaveNavigatedToTheApplication()
        {
            NavigateSite();
            _parallelConfig!.CurrentPage = new HomePage(_parallelConfig);
        }

        [Given(@"I see application opened")]
        public void GivenISeeApplicationOpened()
        {
            _parallelConfig.CurrentPage!.As<HomePage>().CheckIfLoginExist();
        }
        [When(@"I click ""([^""]*)"" link")]
        public void WhenIClickLink(string linkName)
        {
            if (linkName == "login")
                _parallelConfig.CurrentPage = _parallelConfig.CurrentPage!.As<HomePage>().ClickLogin();
        }
        [Then(@"I click ""([^""]*)"" link")]
        public void ThenIClickLink(string linkName)
        {
            if (linkName == "provisioning")
                _parallelConfig.CurrentPage = _parallelConfig.CurrentPage!.As<HomePage>().ClickProvisioning();
        }
        [Then(@"I click ""([^""]*)"" button")]
        public void ThenIClickButton(string buttonName)
        {
            if (buttonName == "login")
                _parallelConfig.CurrentPage = _parallelConfig.CurrentPage!.As<LoginPage>().ClickLoginButton();
            else if (buttonName == "search")
                _parallelConfig.CurrentPage!.As<ProvisioningPge>().ClickonSearch();
        }

        [Then(@"I have navigated to provisioning page")]
        public void ThenIHaveNavigatedToProvisioningPage()
        {
            _parallelConfig.CurrentPage!.As <ProvisioningPge>().CheckIfNavigatedtoProvisioning();
        }
        [Then(@"I select ProvisioningStatus and GroupName")]
        public void ThenISelectProvisioningStatusAndGroupName(Table table)
        {
            dynamic data = table.CreateDynamicInstance();
            _parallelConfig.CurrentPage!.As<ProvisioningPge>().SelectProvisioningStatus(data.ProvisioningStatus);
            _parallelConfig.CurrentPage!.As<ProvisioningPge>().SelectGroupName(data.GroupName);
        }

        [When(@"I enter UserName and Password")]
        public void WhenIEnterUserNameAndPassword(Table table)
        {
            dynamic data = table.CreateDynamicInstance();
            _parallelConfig.CurrentPage!.As<LoginPage>().CheckIfLoginExist();
            _parallelConfig.CurrentPage!.As<LoginPage>().Login(data.UserName, data.Password);
        }

        [Then(@"I should see the username with hello")]
        public void ThenIShouldSeeTheUsernameWithHello()
        {
            if (_parallelConfig.CurrentPage!.As<HomePage>().GetLoggedInUser().Contains("Home"))
                System.Console.WriteLine("Sucessful login");
            else
                System.Console.WriteLine("Unsucessful login");
        }
        [Then(@"I click logout")]
        public void ThenIClickLogout()
        {
            _parallelConfig.CurrentPage!.As<ProvisioningPge>().ClickLogOff();
        }
    }
}
