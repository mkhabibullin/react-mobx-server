using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;

namespace TimeReport.Extensions
{
    public static class WebDriverExtensions
    {
        /// <summary>
        /// An arbitrary wait, guaranteed to wait seconds
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static IWebDriver Wait(this IWebDriver driver, int seconds = 1, int milliseconds = 0)
        {
            var timeToWait = new TimeSpan(0, 0, 0, seconds, milliseconds);
            WebDriverWait wait = new WebDriverWait(driver, timeToWait);
            try
            {
                wait.Until(d => false);
            }
            catch { }
            return driver;
        }

        /// <summary>
        /// Suggested Usage: driver.WaitUntil(d => d.FindElement(By.Id("my-html-id")));
        /// This will use the poll interval set in the BaseTest
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="driver"></param>
        /// <param name="condition"></param>
        /// <param name="seconds">optional param, number of seconds</param>
        /// <returns></returns>
        public static TResult WaitUntil<TResult>(this IWebDriver driver, Func<IWebDriver, TResult> condition, int seconds = 15)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
                return wait.Until(condition);
            }
            catch (Exception exc)
            {
                if(exc is NoSuchElementException || exc is WebDriverTimeoutException)
                    return default(TResult);
                throw;
            }
        }

        /// <summary>
        /// Suggested Usage: driver.WaitForText(element, "the text to match");
        /// This will use the poll interval set in the BaseTest
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="elementName"></param>
        /// <param name="text"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static bool WaitForText(this IWebDriver driver, IWebElement elementName, string text, int seconds = 15)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
            return wait.Until(ExpectedConditions.TextToBePresentInElement(elementName, text));
        }

        /// <summary>
        /// Find the first instance of a element that contains the text
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="by"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static IWebElement ElementContainingText(this IWebDriver driver, By by, string text)
        {
            foreach (var element in driver.FindElements(by))
            {
                if (element.Text.Trim().ToLower() == text.Trim().ToLower())
                    return element;
            }
            return null;
        }

        /// <summary>
        /// Wait for an element to be not displayed
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="htmlId"></param>
        /// <returns></returns>
        public static IWebDriver WaitForIsNotDisplayed(this IWebDriver driver, string htmlId)
        {
            driver.WaitUntil(ExpectedConditions.InvisibilityOfElementLocated(By.Id(htmlId)));
            return driver;
        }

        /// <summary>
        /// Return a visible element on the page where the text matches
        /// Ex: <![CDATA[ <button>Post</button> ]]> can be found by
        /// driver.GetElementOnPageMatching(By.TagName("button"), "Post");
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="baseElement"></param>
        /// <param name="by"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public static IWebElement GetElementOnPageMatching(this IWebDriver driver, IWebElement baseElement, By by, string match)
        {
            // if Exception, try GetElementOnPageContaining()
            return baseElement
                ?.FindElements(by)
                ?.FirstOrDefault(e => e.Displayed && e.Text == match);
        }

        /// <summary>
        /// Return a visible element on the page where the text matches
        /// Ex: <![CDATA[ <button>Post</button> ]]> can be found by
        /// driver.GetElementOnPageMatching(By.TagName("button"), "Post");
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="by"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public static IWebElement GetElementOnPageMatching(this IWebDriver driver, By by, string match)
        {
            return driver.FindElements(by)
                .FirstOrDefault(e => e.Displayed && e.Text == match);
        }

        /// <summary>
        /// Get an element using Contains instead of match
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="baseElement"></param>
        /// <param name="by"></param>
        /// <param name="contains"></param>
        /// <returns></returns>
        public static IWebElement GetElementOnPageContaining(this IWebDriver driver, IWebElement baseElement, By by, string contains)
        {
            return baseElement
                ?.FindElements(by)
                ?.FirstOrDefault(e => e.Displayed && e.Text.Contains(contains));
        }

        public static void ClickLocation(this IWebDriver driver, IWebElement element, int dx = 0, int dy = 0)
        {
            new Actions(driver).MoveToElement(element).MoveByOffset(dx, dy).Click().Perform();
        }
    }
}
