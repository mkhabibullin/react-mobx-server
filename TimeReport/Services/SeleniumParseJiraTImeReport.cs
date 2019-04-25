﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using TimeReport.Configs;
using TimeReport.Dto;
using TimeReport.Extensions;

namespace TimeReport.Services
{
    public class SeleniumParseJiraTImeReport : IParseJiraTimeReport
    {
        private readonly ILogger<SeleniumParseJiraTImeReport> _logger;

        private readonly IOptions<SeleniumConfig> _seleniumConfig;

        public SeleniumParseJiraTImeReport(IOptions<SeleniumConfig> seleniumConfig, ILogger<SeleniumParseJiraTImeReport> logger)
        {
            _seleniumConfig = seleniumConfig;
            _logger = logger;
        }

        public TimeTrackingDto GetTimeTrackingByLink(string url, string email, string pass, DateTime dateFrom, DateTime dateTo)
        {
            var result = new TimeTrackingDto();

            var chromeOptions = new ChromeOptions();
            if(_seleniumConfig.Value.Headless) chromeOptions.AddArguments("headless");

            var chromeDriverPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            _logger.LogInformation($"The path is: {chromeDriverPath}");

            using (var driver = new ChromeDriver(chromeDriverPath, chromeOptions))
            {
                driver.Navigate().GoToUrl(url);

                var emailInput = driver.FindElement(By.Id("username"));
                emailInput.SendKeys(email);

                var nextButton = driver.WaitUntil(d => d.FindElement(By.Id("login-submit")));
                nextButton.Click();

                Thread.Sleep(1000);
                var passwordInput = driver.FindElement(By.Id("password"));
                passwordInput.SendKeys(pass);

                var loginButton = driver.WaitUntil(d => d.FindElement(By.Id("login-submit")));
                loginButton.Click();

                var tbody = driver.WaitUntil(d => d.FindElement(By.CssSelector("tbody")));
                var tasksRows = tbody.FindElements(By.CssSelector("tbody tr"));

                var links = tasksRows
                    .Select(tr => {
                        var link = tr.FindElement(By.CssSelector("a"));
                        var date = tr.FindElement(By.CssSelector("td:nth-of-type(2) div")).Text;
                        return new
                        {
                            title = link.GetAttribute("title"),
                            href = link.GetAttribute("href"),
                            date = date.ParseDate()
                        };
                    }).ToArray();

                links = links
                    .Where(l => l.date.Date >= dateFrom.Date && l.date.Date <= dateTo.Date)
                    .ToArray();

                foreach (var link in links)
                {
                    driver.Navigate().GoToUrl(link.href);

                    var workLoadLink = driver.WaitUntil(d => d.FindElement(By.Id("worklog-tabpanel")));
                    if (workLoadLink != null && workLoadLink.Displayed && workLoadLink.Enabled)
                    {
                        workLoadLink?.Click();
                        Thread.Sleep(200);
                    }

                    var actionContainers = driver
                        .WaitUntil(d => d.FindElements(By.CssSelector(".actionContainer")));

                    if (actionContainers.Any())
                    {
                        var task = new TimeTrackingTaskDto(link.title, link.href);
                        foreach (var ac in actionContainers)
                        {
                            var date = ac.FindElement(By.CssSelector(".action-details span .date"));
                            var spent = GetWorkDuration(link.href, ac);

                            task.Itmes.Add(new TimeTrackingTaskItemDto(DateTime.Parse(date.Text), spent.Text, ""));
                        }
                        result.Tasks.Add(task);
                    }
                }
            }

            return result;
        }

        private IWebElement GetWorkDuration(string link, IWebElement el)
        {
            try
            {
                return el.FindElement(By.CssSelector(".worklog-duration"));
            }
            catch (NoSuchElementException)
            {
                throw new Exception($"Not found class 'worklog-duration' on {link}");
            }
        }
    }
}
