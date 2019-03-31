using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System.Collections.Generic;
using System.Linq;

namespace TimeReport.Tests.Extensions
{
    public static class WebElementExtensions
    {
        /// <summary>
        /// Select a drop down item by the value in the html option attribute value=""
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IWebElement ClickSelectDropDownValue(this IWebElement element, int value)
        {
            element.Click();
            element.FindElements(By.TagName("option"))
                .FirstOrDefault(e => e.GetAttribute("value") == value.ToString())
                .Click();
            return element;
        }

        /// <summary>
        /// Returns true if element is checkbox and checked="checked"
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool IsChecked(this IWebElement element)
        {
            var isChecked = element.GetAttribute("checked")?.ToLower();
            return isChecked == "true" || isChecked == "checked";
        }

        /// <summary>
        /// Check if the element is checked is the same as isChecked and then set it to be the same if not
        /// </summary>
        /// <param name="element"></param>
        /// <param name="isChecked"></param>
        /// <returns></returns>
        public static IWebElement SetChecked(this IWebElement element, bool isChecked)
        {
            var checkedAttribute = element.GetAttribute("checked")?.ToLower();
            var currentIsChecked = checkedAttribute == "true" || checkedAttribute == "checked";
            if (currentIsChecked != isChecked)
                element.Click();
            return element;
        }


        /// <summary>
        /// Get all the column values underneath a given header text
        /// </summary>
        /// <param name="headerName"></param>
        /// <returns></returns>
        public static List<string> GetTableColumnValuesByHeader(this IWebElement table, string headerName)
        {
            var result = new List<string>();
            if (table == null) return result;

            var trs = table.FindElements(By.TagName($"tr"));
            if (trs == null) return result;

            var columnPosition = 0;
            var headerFound = false;

            foreach (var tr in trs)
            {
                if (headerFound == false)
                {
                    var ths = tr.FindElements(By.TagName("th"));
                    foreach (var th in ths)
                    {
                        if (th.Text.Trim().ToLower() == headerName.ToLower())
                        {
                            headerFound = true;
                            break;
                        }
                        else
                        {
                            columnPosition++;
                        }
                    }
                }
                else
                {
                    var tds = tr.FindElements(By.TagName("td"));
                    if (tds.Count >= columnPosition)
                        result.Add(tds[columnPosition].Text.Trim());
                }
            }

            return result;
        }

        /// <summary>
        /// Return a table row by searching the column with headerName for valueToMatch
        /// </summary>
        /// <param name="table"></param>
        /// <param name="headerName"></param>
        /// <param name="valueToMatch"></param>
        /// <returns></returns>
        public static IWebElement GetTableColumnRowByHeaderValue(this IWebElement table, string headerName, string valueToMatch)
        {
            var trs = table.FindElements(By.TagName("tr"));
            var columnPosition = 0;
            var headerFound = false;

            foreach (var tr in trs)
            {
                if (headerFound == false)
                {
                    var ths = tr.FindElements(By.TagName("th"));
                    foreach (var th in ths)
                    {
                        if (th.Text.Trim().ToLower() == headerName.ToLower())
                        {
                            headerFound = true;
                            break;
                        }
                        else
                        {
                            columnPosition++;
                        }
                    }
                }
                else
                {
                    var tds = tr.FindElements(By.TagName("td"));
                    if (tds.Count >= columnPosition && tds[columnPosition].Text.Trim() == valueToMatch)
                        return tr;
                }
            }

            return null;
        }

        public static IWebElement GetParent(this IWebElement element)
        {
            return element.FindElement(By.XPath(".."));
        }
    }
}
