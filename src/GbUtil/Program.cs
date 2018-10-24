using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using GitBucket.Core;
using GitBucket.Data.Repositories;
using GitBucket.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Octokit;
using Octokit.Internal;

namespace GbUtil
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IConfiguration configuration;
            IConsole console = new GbUtilConsole();

            try
            {
                configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true)
                    .AddEnvironmentVariables()
                    .Build();

                var connectionString = configuration.GetConnectionString("GitBucketConnection");
                if (string.IsNullOrEmpty(connectionString))
                {
                    console.WriteWarnLine("PostgreSQL ConnectionString is not configured. Add \"ConnectionStrings: GitBucketConnection\" environment variable.");
                    return;
                }

                var gitbucketUri = configuration.GetSection("GitBucketUri")?.Value;
                if (string.IsNullOrEmpty(gitbucketUri))
                {
                    console.WriteWarnLine("GitBucket URI is not configured. Add \"GitBucketUri\" environment variable.");
                    return;
                }

                var serviceProvider = new ServiceCollection()
                    .AddScoped<DbContext>(_ => new GitBucketDbContext(connectionString))
                    .AddTransient<IReleaseNoteService, ReleaseNoteService>()
                    .AddTransient<IMilestoneService, MilestoneService>()
                    .AddTransient<IIssueService, IssueService>()
                    .AddTransient<IssueRepositoryBase, IssueRepository>()
                    .AddTransient<LabelRepositoryBase, LabelRepository>()
                    .AddTransient<MilestoneRepositoryBase, MilestoneRepository>()
                    .AddTransient<IConsole, GbUtilConsole>()
                    .BuildServiceProvider();

                using (var scope = serviceProvider.CreateScope())
                {
                    var provider = scope.ServiceProvider;
                    await Parser.Default.ParseArguments<ReleaseOptions, MilestoneOptions, IssueOptions>(args)
                        .MapResult(
                            (ReleaseOptions options) => provider.GetRequiredService<IReleaseNoteService>().OutputReleaseNotes(options),
                            (MilestoneOptions options) => provider.GetRequiredService<IMilestoneService>().ShowMilestones(options),
                            (IssueOptions options) =>
                            {
                                console.Write("Enter your Username: ");
                                string user = console.ReadLine();
                                if (string.IsNullOrEmpty(user))
                                {
                                    return Task.FromResult(1);
                                }

                                console.Write("Enter your Password: ");
                                string password = GetConsolePassword();
                                if (string.IsNullOrEmpty(password))
                                {
                                    return Task.FromResult(1);
                                }

                                var client = new GitHubClient(
                                    new ProductHeaderValue("gbutil"),
                                    new InMemoryCredentialStore(new Credentials(user, password)),
                                    new Uri(gitbucketUri));

                                return provider.GetRequiredService<IIssueService>().MoveIssue(options, client);
                            },
                            errs => Task.FromResult(-1));
                }
            }
            catch (Exception ex)
            {
                console.WriteErrorLine(ex.Message);
                console.WriteErrorLine(ex.StackTrace);
            }
        }

        private static string GetConsolePassword()
        {
            var builder = new StringBuilder();
            while (true)
            {
                var consoleKeyInfo = Console.ReadKey(true);
                if (consoleKeyInfo.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }

                if (consoleKeyInfo.Key == ConsoleKey.Backspace)
                {
                    if (builder.Length > 0)
                    {
                        Console.Write("\b\0\b");
                        builder.Length--;
                    }

                    continue;
                }

                if (((int)consoleKeyInfo.Key) >= 65 && ((int)consoleKeyInfo.Key <= 90))
                {
                    Console.Write('*');
                    builder.Append(consoleKeyInfo.KeyChar);
                }                
            }

            return builder.ToString();
        }
    }
}
