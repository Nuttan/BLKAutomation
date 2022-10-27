using BLKAutoFramework.Helpers;
using BLKAutoFramework.Utility;
using BoDi;
using OpenQA.Selenium.Chrome;
using TechTalk.SpecFlow;
using OpenQA.Selenium.Remote;

namespace BLKAutoFramework.Base
{
    public class TestInitializeHook : Steps
    {

        private readonly ParallelConfig _parallelConfig;
        private IFeaturesConfiguration _featuresConfiguration;
        private readonly IObjectContainer _objectContainer;

        public TestInitializeHook(ParallelConfig parallelConfig, IObjectContainer objectContainer)
        {
            _parallelConfig = parallelConfig;
            _objectContainer = objectContainer;
        }
        public void InitializeSettings()
        {
            _featuresConfiguration = _objectContainer.Resolve<IFeaturesConfiguration>();
            BrowserType browserType = (BrowserType)Enum.Parse(typeof(BrowserType), _featuresConfiguration.BrowserType);
            //Set all the settings for framework
            //Set Log
            LogHelpers.CreateLogFile();
            //Open Browser
            OpenBrowser(browserType);
            LogHelpers.Write("Initialized framework");
        }
        private void OpenBrowser(BrowserType browserType )
        {
            var chromeOption = new ChromeOptions();
             chromeOption.AddArguments(new List<string>() {
    "--silent-launch",
    "--no-startup-window",
    "no-sandbox",
    "headless",});
            chromeOption.AddArguments("disable-infobars");
           // chromeOption.AddArguments("-headless");
            chromeOption.AddArguments("window-size=1920,1080");
            _parallelConfig.Driver = new RemoteWebDriver(new Uri("http://docker:4444/wd/hub/"), chromeOption.ToCapabilities());

           //InternetExplorerOptions options = new InternetExplorerOptions();
           //options.AddAdditionalCapability("ignoreProtectedModeSettings", true);
           //options.AddAdditionalCapability("EnsureCleanSession", true);
           //_parallelConfig.Driver = new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub"), options.ToCapabilities(), TimeSpan.FromSeconds(300));
        }
    }
}
