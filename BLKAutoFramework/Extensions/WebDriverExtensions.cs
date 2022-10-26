using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System.Diagnostics;
using OpenQA.Selenium.Support.UI;

namespace BLKAutoFramework.Extensions
{
    public static class WebDriverExtensions
    {
        public static void WaitForPageLoaded(this IWebDriver driver)
        {
            driver.WaitForCondition(dri =>
            {
                var state = ((IJavaScriptExecutor)dri).ExecuteScript("return document.readyState").ToString();
                return state == "complete";
            }, 10);
        }
        public static void WaitForCondition<T>(this T obj, Func<T, bool> condition, int timeOut)
        {
            Func<T, bool> execute =
                (arg) =>
                {
                    try
                    {
                        return condition(arg);
                    }
                    catch (Exception e)
                    {
                        throw new TimeoutException(e.Message);
                    }
                };
            var stopWatch = Stopwatch.StartNew();
            while (stopWatch.ElapsedMilliseconds < timeOut)
            {
                if (execute(obj))
                {
                    break;
                }
            }
        }
        public static IWebElement? FindById(this RemoteWebDriver remoteWebDriver, string element)
        {
            WebDriverWait w = new WebDriverWait(remoteWebDriver, TimeSpan.FromSeconds(10));
            try
            {
                if (w.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id(element))).IsElementPresent())
                {
                    return w.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id(element)));
                    //return remoteWebDriver.FindElementById(element);
                }
            }
            catch (Exception e)
            {
                throw new ElementNotVisibleException($"Element not found : {element}" + e.Message);
            }
            return null;
        }
        public static IWebElement? FindByXpath(this RemoteWebDriver remoteWebDriver, string element)
        {
            WebDriverWait w = new WebDriverWait(remoteWebDriver, TimeSpan.FromSeconds(15));
            try
            {
                if (w.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath(element))).IsElementPresent())
                {
                    return w.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath(element)));
                    //return remoteWebDriver.FindElementByXPath(element);
                }
            }
            catch (Exception e)
            {
                throw new ElementNotVisibleException($"Element not found : {element}" + e.Message);
            }
            return null;
        }
        public static IWebElement? FindByCss(this RemoteWebDriver remoteWebDriver, string element)
        {
            try
            {
                if (remoteWebDriver.FindElementByCssSelector(element).IsElementPresent())
                {
                    return remoteWebDriver.FindElementByCssSelector(element);
                }
            }
            catch (Exception e)
            {
                throw new ElementNotVisibleException($"Element not found : {element}" + e.Message);
            }
            return null;
        }
        public static IWebElement? FindByLinkText(this RemoteWebDriver remoteWebDriver, string element)
        {
            try
            {
                if (remoteWebDriver.FindElementByLinkText(element).IsElementPresent())
                {
                    return remoteWebDriver.FindElementByLinkText(element);
                }
            }
            catch (Exception e)
            {
                throw new ElementNotVisibleException($"Element not found : {element}" + e.Message);
            }
            return null;
        }
    }
}
