using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using TimeReport.Services;
using TimeReport.Tests.Extensions;

namespace TimeReport.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("headless");

            var url = Environment.GetEnvironmentVariable("TestReportUrl", EnvironmentVariableTarget.Machine);
            var email = Environment.GetEnvironmentVariable("TestReportEmail", EnvironmentVariableTarget.User);
            var pass = Environment.GetEnvironmentVariable("TestReportPass", EnvironmentVariableTarget.User);

            using (var driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), chromeOptions))
            {
                driver.Navigate().GoToUrl(url);

                var emailInput = driver.FindElement(By.Id("username"));
                emailInput.SendKeys(email);

                var button = driver.WaitUntil(d => d.FindElement(By.Id("login-submit")));
                button.Click();
                
                Thread.Sleep(1000);
                var passwordInput = driver.FindElement(By.Id("password"));
                passwordInput.SendKeys(pass);

                var button2 = driver.WaitUntil(d => d.FindElement(By.Id("login-submit")));
                button2.Click();

                var tbody = driver.WaitUntil(d => d.FindElement(By.CssSelector("tbody")));
                var links = tbody.FindElements(By.CssSelector("tbody a"));

                var linksData = links
                    .Select(e => new
                    {
                        title = e.GetAttribute("title"),
                        href = e.GetAttribute("href")
                    }).ToList();

                foreach (var link in linksData)
                {
                    Trace.WriteLine(link.title);
                    Trace.WriteLine(link.href);
                    driver.Navigate().GoToUrl(link.href);

                    var actionContainers = driver
                        .WaitUntil(d => d.FindElements(By.CssSelector(".actionContainer")));
                    foreach(var ac in actionContainers)
                    {
                        var date = ac.FindElement(By.CssSelector(".action-details span .date"));
                        Trace.WriteLine(DateTime.Parse(date.Text));
                        var spend = ac.FindElement(By.CssSelector(".worklog-duration"));
                        Trace.WriteLine(spend.Text);
                    }
                }
            }
        }

        [TestMethod]
        public void ParseJiraTimeReportServiceTest()
        {
            var service = new SeleniumParseJiraTImeReport();

            var url = Environment.GetEnvironmentVariable("TestReportUrl", EnvironmentVariableTarget.Machine);
            var email = Environment.GetEnvironmentVariable("TestReportEmail", EnvironmentVariableTarget.User);
            var pass = Environment.GetEnvironmentVariable("TestReportPass", EnvironmentVariableTarget.User);

            var report = service.GetReportByLink(url, email, pass);

            foreach(var t in report.Tasks)
            {
                Trace.WriteLine(t.Name);
                Trace.WriteLine(t.Link);

                foreach(var i in t.Itmes)
                {
                    Trace.WriteLine(i.Date);
                    Trace.WriteLine(i.TimeSpent);
                }
            }
        }
    }
}
