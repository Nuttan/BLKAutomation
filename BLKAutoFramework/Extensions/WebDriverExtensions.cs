using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System.Diagnostics;
using OpenQA.Selenium.Chrome;
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
        public static IWebElement? FindById(this ChromeDriver chromeDriver, string element)
        {
            WebDriverWait w = new WebDriverWait(chromeDriver, TimeSpan.FromSeconds(10));
            try
            {
                if (w.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id(element))).IsElementPresent())
                {
                    return w.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id(element)));
                }
            }
            catch (Exception e)
            {
                throw new ElementNotVisibleException($"Element not found : {element}" + e.Message);
            }
            return null;
        }

        public static IWebElement? FindElementByClassName(this ChromeDriver chromeDriver, string element)
        {
            WebDriverWait w = new WebDriverWait(chromeDriver, TimeSpan.FromSeconds(10));
            try
            {
                if (w.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.ClassName(element))).IsElementPresent())
                {
                    return w.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.ClassName(element)));
                }
            }
            catch (Exception e)
            {
                throw new ElementNotVisibleException($"Element not found : {element}" + e.Message);
            }
            return null;
        }
        public static IWebElement? FindByXpath(this ChromeDriver chromeDriver, string element)
        {
            WebDriverWait w = new WebDriverWait(chromeDriver, TimeSpan.FromSeconds(15));
            try
            {
                if (w.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath(element))).IsElementPresent())
                {
                    return w.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath(element)));
                }
            }
            catch (Exception e)
            {
                throw new ElementNotVisibleException($"Element not found : {element}" + e.Message);
            }
            return null;
        }
        public static IWebElement? FindByCss(this ChromeDriver chromeDriver, string element)
        {
            try
            {
                if ((chromeDriver.FindElement(By.CssSelector(element)).IsElementPresent()))
                {
                    return chromeDriver.FindElement(By.CssSelector(element));
                }
            }
            catch (Exception e)
            {
                throw new ElementNotVisibleException($"Element not found : {element}" + e.Message);
            }
            return null;
        }
        public static IWebElement? FindByLinkText(this ChromeDriver chromeDriver, string element)
        {
            try
            {
                if (chromeDriver.FindElement(By.LinkText(element)).IsElementPresent())
                {
                    return chromeDriver.FindElement(By.LinkText(element));
                }
            }
            catch (Exception e)
            {
                throw new ElementNotVisibleException($"Element not found : {element}" + e.Message);
            }
            return null;
        }
        public static SelectElement FindSelectElementWhenPopulated(this ChromeDriver chromeDriver, string locator)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(chromeDriver, TimeSpan.FromSeconds(20));
                Func<IWebDriver, SelectElement> condition = drv =>
                                    {
                                        SelectElement element = new SelectElement(drv.FindElement(By.XPath(locator)));
                                        if (element.Options.Count >= 2)
                                        {
                                            return element;
                                        }
                                        return null!;
                                    };
                return wait.Until<SelectElement>(condition
                );
            }
            catch (Exception e)
            {
                throw new ElementNotSelectableException($"Element not found : {locator}" + e.Message);
            }
        }
    }
}
