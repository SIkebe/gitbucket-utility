using System;
using System.IO;
using System.Linq;
using CommandLine;
using GitBucket.Core;
using GitBucket.Core.Models;
using GitBucket.Data.Repositories;
using GitBucket.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GbUtil
{
    class Program
    {

        static void Main(string[] args)
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

                var serviceProvider = new ServiceCollection()
                    .AddScoped<DbContext>(_ => new GitBucketDbContext(connectionString))
                    .AddTransient<IReleaseNoteService, ReleaseNoteService>()
                    .AddTransient<IMilestoneService, MilestoneService>()
                    .AddTransient<IssueRepositoryBase, IssueRepository>()
                    .AddTransient<LabelRepositoryBase, LabelRepository>()
                    .AddTransient<MilestoneRepositoryBase, MilestoneRepository>()
                    .AddTransient<IConsole, GbUtilConsole>()
                    .BuildServiceProvider();

                using (var scope = serviceProvider.CreateScope())
                {
                    var provider = scope.ServiceProvider;
                    Parser.Default.ParseArguments<ReleaseOptions, MilestoneOptions>(args)
                        .MapResult(
                            (ReleaseOptions options) => provider.GetRequiredService<IReleaseNoteService>().OutputReleaseNotes(options),
                            (MilestoneOptions options) => provider.GetRequiredService<IMilestoneService>().ShowMilestones(options),
                            errs => 1);
                }
            }
            catch (Exception ex)
            {
                console.WriteErrorLine(ex.Message);
                console.WriteErrorLine(ex.StackTrace);
            }
        }
    }
}
