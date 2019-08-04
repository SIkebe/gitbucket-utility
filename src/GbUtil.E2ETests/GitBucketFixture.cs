using System;
using System.Diagnostics;
using System.Threading.Tasks;
using GitBucket.Core;
using Octokit;
using Octokit.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;

namespace GbUtil.E2ETests
{
    public class GitBucketFixture : IDisposable, IAsyncLifetime
    {
        private bool disposedValue = false;

        public GitBucketFixture()
        {
            Debug.WriteLine("test1");

            Environment.SetEnvironmentVariable("GbUtil_ConnectionStrings", GitBucketDefaults.ConnectionStrings);
            Environment.SetEnvironmentVariable("GbUtil_GitBucketUri", GitBucketDefaults.ApiEndpoint);
            Environment.SetEnvironmentVariable("GbUtil_UserName", GitBucketDefaults.Owner);
            Environment.SetEnvironmentVariable("GbUtil_Password", GitBucketDefaults.Password);

            GitBucketClient = new GitHubClient(new Connection(
                    new ProductHeaderValue("gbutil"),
                    new Uri(GitBucketDefaults.ApiEndpoint),
                    new InMemoryCredentialStore(new Credentials(GitBucketDefaults.Owner, GitBucketDefaults.Password)),
                    new HttpClientAdapter(() => new GitBucketMessageHandler()),
                    new SimpleJsonSerializer()
                ));

            Driver = CreateChromeDriver();
            Driver.Navigate().GoToUrl(new Uri(GitBucketDefaults.BaseUri));

            Driver.FindElement(By.Id("signin")).Click();

            Driver.FindElement(By.Id("userName")).Clear();
            Driver.FindElement(By.Id("userName")).SendKeys(GitBucketDefaults.Owner);

            Driver.FindElement(By.Id("password")).Clear();
            Driver.FindElement(By.Id("password")).SendKeys(GitBucketDefaults.Password);

            Driver.FindElement(By.XPath("/html/body/div/div/div/div/div/ul/li/form/input[2]")).Click();
        }

        ~GitBucketFixture()
        {
            Dispose(false);
        }

        public IWebDriver Driver { get; set; }
        public IGitHubClient GitBucketClient { get; set; }

        public static IWebDriver CreateChromeDriver()
        {
            var opts = new ChromeOptions();

            // Comment this out if you want to watch or interact with the browser (e.g., for debugging)
            if (!Debugger.IsAttached)
            {
                //opts.AddArgument("--headless");
            }

            var driver = new ChromeDriver(ChromeDriverService.CreateDefaultService(), opts, TimeSpan.FromSeconds(60));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
            return driver;
        }

        public static string Execute(string arguments)
        {
            using var process = new Process();
            process.StartInfo.FileName = "dotnet";
            process.StartInfo.Arguments = @$"""{GitBucketDefaults.GbUtilDll}"" {arguments}";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;

            process.Start();

            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output;
        }

        public async Task InitializeAsync()
        {
            await GitBucketClient.Repository.Create(new NewRepository(GitBucketDefaults.Repository1) { AutoInit = true });
            await GitBucketClient.Repository.Create(new NewRepository(GitBucketDefaults.Repository2) { AutoInit = true });
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Called just before Dispose().
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task DisposeAsync()
        {
            // Not Implemented.
            // await GitBucketClient.Repository.Delete(GitBucketDefaults.Owner, GitBucketDefaults.Repository);
            return Task.CompletedTask;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                Driver?.Dispose();
                disposedValue = true;
            }
        }
    }
}
