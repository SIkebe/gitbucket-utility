using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using GbUtil.Extensions;
using GitBucket.Core;
using GitBucket.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Octokit;
using Octokit.Internal;

namespace GbUtil
{
    public sealed class Program
    {
        public static async Task<int> Main(string[] args)
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

                var options = Parser.Default.ParseArguments<ReleaseOptions, MilestoneOptions, IssueOptions>(args)
                    .WithNotParsed(errors =>
                    {
                        if (errors.Any(e =>
                            e.Tag != ErrorType.HelpVerbRequestedError &&
                            e.Tag != ErrorType.VersionRequestedError &&
                            e.Tag != ErrorType.NoVerbSelectedError))
                        {
                            throw new InvalidConfigurationException($"Failed to parse arguments.");
                        }
                    })
                    .MapResult<ReleaseOptions, MilestoneOptions, IssueOptions, CommandLineOptionsBase?>(
                        (ReleaseOptions options) => options,
                        (MilestoneOptions options) => options,
                        (IssueOptions options) => options,
                        _ => null
                    );

                // In case of default verbs (--help or --version)
                if (options == null)
                {
                    return 0;
                }

                var requireDbConnection = options is ReleaseOptions || options is MilestoneOptions;
                using var scope = CreateServiceProvider(configuration, requireDbConnection).CreateScope();
                var result = options switch
                {
                    ReleaseOptions releaseOptions
                        => await scope.ServiceProvider.GetRequiredService<IReleaseService>().Execute(releaseOptions, CreateGitBucketClient(configuration, console)),
                    MilestoneOptions milestoneOptions
                        => await scope.ServiceProvider.GetRequiredService<IMilestoneService>().ShowMilestones(milestoneOptions),
                    IssueOptions issueOptions
                        => await scope.ServiceProvider.GetRequiredService<IIssueService>().Execute(issueOptions, CreateGitBucketClient(configuration, console)),
                    _ => 1
                };

                return result;
            }
            catch (InvalidConfigurationException ex)
            {
                console.WriteWarnLine(ex.Message);
                return 1;
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
            {
                console.WriteErrorLine(ex.Message);
                console.WriteErrorLine(ex.StackTrace);
                return 1;
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }

        private static ServiceProvider CreateServiceProvider(
            IConfiguration configuration,
            bool requireDbConnection = false)
        {
            string? connectionString = "";
            if (requireDbConnection)
            {
                connectionString = configuration.GetSection("GbUtil_ConnectionStrings")?.Value;
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidConfigurationException("PostgreSQL ConnectionString is not configured. Add \"GbUtil_ConnectionStrings\" environment variable.");
                }
            }

            return new ServiceCollection()
                .AddScopedIf<DbContext>(requireDbConnection, _ => new GitBucketDbContext(connectionString))
                .AddTransient<IReleaseService, ReleaseService>()
                .AddTransient<IMilestoneService, MilestoneService>()
                .AddTransient<IIssueService, IssueService>()
                .AddTransient<IConsole, GbUtilConsole>()
                .BuildServiceProvider();
        }

        private static IGitHubClient CreateGitBucketClient(IConfiguration configuration, IConsole console)
        {
            var gitbucketUri = configuration.GetSection("GbUtil_GitBucketUri")?.Value;
            if (string.IsNullOrEmpty(gitbucketUri))
            {
                throw new InvalidConfigurationException("GitBucket URI is not configured. Add \"GbUtil_GitBucketUri\" environment variable.");
            }

            Credentials credentials;
            var token = configuration.GetSection("GbUtil_AccessToken")?.Value;
            if (!string.IsNullOrEmpty(token))
            {
                credentials = new Credentials(token);
            }
            else
            {
                var user = configuration.GetSection("GbUtil_UserName")?.Value;
                if (string.IsNullOrEmpty(user))
                {
                    console.Write("Enter your Username: ");
                    user = console.ReadLine();
                    if (string.IsNullOrEmpty(user))
                    {
                        throw new InvalidConfigurationException("Username is required");
                    }
                }

                var password = configuration.GetSection("GbUtil_Password")?.Value;
                if (string.IsNullOrEmpty(password))
                {
                    console.Write("Enter your Password: ");
                    password = console.GetPassword();
                    if (string.IsNullOrEmpty(password))
                    {
                        throw new InvalidConfigurationException("Password is required");
                    }
                }

                credentials = new Credentials(user, password);
            }

            return new GitHubClient(
                new Connection(
                    new ProductHeaderValue("gbutil"),
                    new Uri(gitbucketUri),
                    new InMemoryCredentialStore(credentials),
                    new HttpClientAdapter(() => new GitBucketMessageHandler()),
                    new SimpleJsonSerializer()
                ));
        }
    }
}
