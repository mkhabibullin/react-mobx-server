using AutoMapper;
using CodeSuperior.PipelineStyle;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NSpecifications;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using TimeReport.Configs;
using TimeReport.Dto;
using TimeReport.Dto.Jira;
using TimeReport.Extensions;

namespace TimeReport.Services
{
    public class SeleniumParseJiraTImeReport : IParseJiraTimeReport
    {
        private readonly ILogger<SeleniumParseJiraTImeReport> _logger;
        private readonly SeleniumConfig _seleniumConfig;
        private readonly IMapper _mapper;

        private string ChromeDriverPath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        private ChromeOptions ChromeOptions => new ChromeOptions()
            .DoIf(_seleniumConfig.Headless, AsHeadless);

        public SeleniumParseJiraTImeReport(IOptions<SeleniumConfig> seleniumConfig, ILogger<SeleniumParseJiraTImeReport> logger, IMapper mapper)
        {
            _seleniumConfig = seleniumConfig.Value;
            _logger = logger;
            _mapper = mapper;
        }

        public TimeTrackingDto GetTimeTrackingByLink(string url, string email, string pass, DateTime dateFrom, DateTime dateTo)
        {
            var result = new TimeTrackingDto();

            using (var driver = new ChromeDriver(ChromeDriverPath, ChromeOptions))
            {
                // 1. Login
                url
                    .Do(NavigateToUrl)
                    .Do(_ => FillInput(driver, "username", email))
                    .Do(_ => SleepOneSecond()) // waiting for the password field rendering
                    .Do(_ => FillInput(driver, "password", pass));

                // 2. Extracting links of tasks
                var links = driver
                    .To(GetTasksUrls)
                    .To(NormalizeTasksUrls)
                    .To(FilterByDateInterval)
                    .ToArray(); 

                foreach (var link in links)
                {
                    try
                    {
                        var task = new TimeTrackingTaskDto(link.Title, link.Href);

                        // 3. Geting work logs
                        var jiraWorkLogsData = link
                            .To(FindTaskNum)
                            .To(GetWorkLogUrl)
                            .Do(NavigateToUrl)
                            .To(_ => driver.FindElement(By.TagName("body")).Text)
                            .To(JsonConvert.DeserializeObject<JiraWorkLogsDto>);

                        // 4. Mapping to model
                        task.Itmes = jiraWorkLogsData.Worklogs
                            .Select(_mapper.Map<TimeTrackingTaskItemDto>)
                            .ToList();

                        result.Tasks.Add(task);
                    }
                    catch (Exception exc)
                    {
                        _logger.LogCritical($"Error in parsing the page {link}", exc);
                    }
                }

                #region Local WebDriver functions

                void NavigateToUrl(string link) => driver.Navigate().GoToUrl(link);

                #endregion
            }

            return result;

            #region Local functions

            IEnumerable<NormalizedRow> FilterByDateInterval(IEnumerable<NormalizedRow> rows)
                => rows.Where(NormalizedRow.IsInBetween(dateFrom, dateTo));

            #endregion
        }

        #region Helpers

        private void AsHeadless(ChromeOptions chromeOptions) => chromeOptions.AddArguments("headless");

        private string GetWorkLogUrl(string taskNum) =>
            $"https://aware360platformdev.atlassian.net/rest/internal/3/issue/{taskNum}/worklog?startAt=0";

        private string FindTaskNum(NormalizedRow link) => 
            link.Href.Split('/', StringSplitOptions.RemoveEmptyEntries)
            .Last();

        private void FillInput(IWebDriver driver, string inputId, string value)
        {
            driver
                .WaitUntil(d => d.FindElement(By.Id(inputId)))
                .Do(input => value.Do(input.SendKeys));

            driver
                .WaitUntil(d => d.FindElement(By.Id("login-submit")))
                .Do(button => button.Click());
        }

        private IReadOnlyCollection<IWebElement> GetTasksUrls(IWebDriver driver) => 
            driver
            .WaitUntil(d => d.FindElement(By.CssSelector("tbody")))
            .To(tbody => tbody.FindElements(By.CssSelector("tr")));

        /// <summary>
        /// Converts IWebElement[] -> NormalizedRows[]
        /// </summary>
        private IEnumerable<NormalizedRow> NormalizeTasksUrls(IReadOnlyCollection<IWebElement> rows) =>
            rows.Select(tr =>
            {
                var link = tr.FindElement(By.CssSelector("a"));
                var date = tr.FindElement(By.CssSelector("td:nth-of-type(2) div")).Text;
                return new NormalizedRow(link.GetAttribute("title"), link.GetAttribute("href"), date.ParseDate());
            });

        private void SleepOneSecond() => Thread.Sleep(1000);

        private struct NormalizedRow
        {
            public string Title { get; }
            public string Href { get; }
            public DateTime Date { get; }

            public NormalizedRow(string title, string href, DateTime date)
            {
                Title = title;
                Href = href;
                Date = date;
            }

            public static Spec<NormalizedRow> IsInBetween(DateTime dateFrom, DateTime dateTo) =>
                new Spec<NormalizedRow>(row => row.Date.Date >= dateFrom.Date && row.Date.Date <= dateTo.Date);
        }

        #endregion
    }
}
