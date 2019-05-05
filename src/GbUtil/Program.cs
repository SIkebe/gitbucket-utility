﻿using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using GbUtil.Extensions;
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
#nullable disable

                // TODO: CommandLineOptionsBase? does not work here...
                CommandLineOptionsBase options = Parser.Default.ParseArguments<ReleaseOptions, MilestoneOptions, IssueOptions>(args)
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
                    .MapResult(
                        (ReleaseOptions options) => (CommandLineOptionsBase)options,
                        (MilestoneOptions options) => options,
                        (IssueOptions options) => options,
                        _ => null
                    );
#nullable restore

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
            string connectionString = "";
            if (requireDbConnection)
            {
                connectionString = configuration.GetConnectionString("GitBucketConnection");
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidConfigurationException("PostgreSQL ConnectionString is not configured. Add \"ConnectionStrings: GitBucketConnection\" environment variable.");
                }
            }

            return new ServiceCollection()
                .AddScopedIf<DbContext>(requireDbConnection, _ => new GitBucketDbContext(connectionString))
                .AddTransient<IReleaseService, ReleaseService>()
                .AddTransient<IMilestoneService, MilestoneService>()
                .AddTransient<IIssueService, IssueService>()
                .AddTransientIf<IssueRepositoryBase, IssueRepository>(requireDbConnection)
                .AddTransientIf<LabelRepositoryBase, LabelRepository>(requireDbConnection)
                .AddTransientIf<MilestoneRepositoryBase, MilestoneRepository>(requireDbConnection)
                .AddTransient<IConsole, GbUtilConsole>()
                .BuildServiceProvider();
        }

        private static IGitHubClient CreateGitBucketClient(IConfiguration configuration, IConsole console)
        {
            var gitbucketUri = configuration.GetSection("GitBucketUri")?.Value;
            if (string.IsNullOrEmpty(gitbucketUri))
            {
                throw new InvalidConfigurationException("GitBucket URI is not configured. Add \"GitBucketUri\" environment variable.");
            }

            console.Write("Enter your Username: ");
            string user = console.ReadLine();
            if (string.IsNullOrEmpty(user))
            {
                throw new InvalidConfigurationException("Username is required");
            }

            console.Write("Enter your Password: ");
            string password = console.GetPassword();
            if (string.IsNullOrEmpty(password))
            {
                throw new InvalidConfigurationException("Password is required");
            }

            return new GitHubClient(
                new Connection(
                    new ProductHeaderValue("gbutil"),
                    new Uri(gitbucketUri),
                    new InMemoryCredentialStore(new Credentials(user, password)),
                    new HttpClientAdapter(() => new GitBucketMessageHandler()),
                    new SimpleJsonSerializer()
                ));
        }
    }

    public class GitBucketMessageHandler : DelegatingHandler
    {
        public GitBucketMessageHandler() : base(new HttpClientHandler())
        {
        }

        protected async override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken = default)
        {
            if (request != null && request.Content != null)
            {
                var contentType = request.Content.Headers.ContentType.MediaType;
                if (contentType == "application/x-www-form-urlencoded")
                {
                    // GitBucket doesn't accept Content-Type: application/x-www-form-urlencoded
                    request.Content.Headers.ContentType.MediaType = "application/json";
                }
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
