using AventStack.ExtentReports;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace BLKAutoFramework.Base
{
    public class ParallelConfig
    {

        public RemoteWebDriver? Driver { get; set; }

        public BasePage? CurrentPage { get; set; }

        public MediaEntityModelProvider CaptureScreenshotAndReturnModel(string Name)
        {
            var screenshot = ((ITakesScreenshot)Driver!).GetScreenshot().AsBase64EncodedString;
            return MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshot,Name).Build();
        }

    }
}
