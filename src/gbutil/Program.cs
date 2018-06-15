using System;
using System.IO;
using System.Linq;
using CommandLine;
using GbUtil.Extensions;
using GitBucket.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GbUtil
{
    class Program
    {
        public static IConfiguration Configuration { get; set; }

        static void Main(string[] args)
        {
            try
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true)
                    .AddEnvironmentVariables();

                Configuration = builder.Build();

                var connectionString = Configuration.GetConnectionString("GitBucketConnection");
                if (string.IsNullOrEmpty(connectionString))
                {
                    Console.WriteLine("PostgreSQL ConnectionString is not configured. Add \"ConnectionStrings: GitBucketConnection\" environment variable.");
                    return;
                }

                Parser.Default.ParseArguments<ReleaseOptions>(args)
                    .WithParsed(options => OutputReleaseNotes(options, connectionString));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static int OutputReleaseNotes(ReleaseOptions option, string connectionString)
        {
            using (var context = new GitBucketDbContext(connectionString))
            {
                var issues = context.Issue
                .Where(i => i.UserName == option.Owner)
                .Where(i => i.RepositoryName == option.Repository)
                .Where(i => i.Milestone.Title == option.MileStone)
                .Where(i => !i.PullRequest)
                .Include(i => i.Milestone)
                .ToList();

                if (issues.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"There are no issues related to \"{option.MileStone}\".");
                    Console.ResetColor();
                    return 1;
                }

                if (issues.Any(i => !i.Closed))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"There are unclosed issues in \"{option.MileStone}\".");
                    Console.Write("Do you want to continue?([Y]es/[N]o): ");
                    Console.ResetColor();

                    string yesOrNo = Console.ReadLine();
                    
                    if (!string.Equals(yesOrNo, "y", StringComparison.OrdinalIgnoreCase)
                    && !string.Equals(yesOrNo, "yes", StringComparison.OrdinalIgnoreCase))
                    {
                        return 1;
                    }

                    Console.WriteLine("");
                }

                var issueLabels = context.IssueLabel
                .Where(l => l.UserName == option.Owner)
                .Where(l => l.RepositoryName == option.Repository)
                .Where(l => issues.Select(i => i.IssueId).Contains(l.IssueId));

                if (issues.Any(i => !issueLabels.Select(l => l.IssueId).Contains(i.IssueId)))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"There are issues which have no labels in \"{option.MileStone}\".");
                    Console.ResetColor();
                    return 1;
                }

                var labels = context.Label
                .Where(l => l.UserName == option.Owner)
                .Where(l => l.RepositoryName == option.Repository)
                .Where(l => issueLabels.Select(i => i.LabelId).Contains(l.LabelId));

                Console.WriteLine($"As part of this release we had {issues.Count} issues closed.");
                Console.WriteLine("");
                foreach (var label in labels)
                {
                    Console.WriteLine($"### {label.LabelName.ConvertFirstCharToUpper()}");

                    var ids = issueLabels
                    .Where(l => l.LabelId == label.LabelId)
                    .Select(i => i.IssueId)
                    .OrderBy(i => i);

                    foreach (var issueId in ids)
                    {
                        var issue = issues.Where(i => i.IssueId == issueId).Single();
                        Console.WriteLine($"* {issue.Title} #{issue.IssueId}");
                    }

                    Console.WriteLine("");
                }

                return 0;
            }
        }
    }

    [Verb("release", HelpText = "Output release notes")]
    public class ReleaseOptions
    {
        [Option('o', "owner", Required = true, HelpText = "The owner name of the repository")]
        public string Owner { get; set; }

        [Option('r', "repository", Required = true, HelpText = "The repository name")]
        public string Repository { get; set; }

        [Option('m', "milestone", Required = true, HelpText = "The milestone to publish release notes")]
        public string MileStone { get; set; }
    }
}
