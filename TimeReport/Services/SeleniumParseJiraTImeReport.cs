﻿using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using TimeReport.Dto;
using TimeReport.Extensions;

namespace TimeReport.Services
{
    public class SeleniumParseJiraTImeReport : IParseJiraTimeReport
    {
        public TimeReportDto GetReportByLink(string url, string email, string pass)
        {
            var result = new TimeReportDto();

            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("headless");

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
                    driver.Navigate().GoToUrl(link.href);

                    var actionContainers = driver
                        .WaitUntil(d => d.FindElements(By.CssSelector(".actionContainer")));

                    if (actionContainers.Any())
                    {
                        var task = new TimeReportTaskDto(link.title, link.href);
                        foreach (var ac in actionContainers)
                        {
                            var date = ac.FindElement(By.CssSelector(".action-details span .date"));
                            var spent = ac.FindElement(By.CssSelector(".worklog-duration"));

                            task.Itmes.Add(new TimeReportTaskItemDto(DateTime.Parse(date.Text), spent.Text, ""));
                        }
                        result.Tasks.Add(task);
                    }
                }
            }

            return result;
        }
    }
}