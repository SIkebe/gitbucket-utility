using System;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using gbutil.Extensions;
using gbutil.Models;
using Microsoft.EntityFrameworkCore;
using Octokit;

namespace gbutil
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var outputReleasenotes = new Action<Options>((option) =>
                {
                    using (var context = new GitBucketDbContext())
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
                            Console.WriteLine($"There are no issues related to \"{option.MileStone}\".");
                            return;
                        }

                        if (issues.Any(i => !i.Closed))
                        {
                            Console.WriteLine($"There are unclosed issues in \"{option.MileStone}\".");
                            return;
                        }

                        var issueLabels = context.IssueLabel
                        .Where(l => l.UserName == option.Owner)
                        .Where(l => l.RepositoryName == option.Repository)
                        .Where(l => issues.Select(i => i.IssueId).Contains(l.IssueId))
                        .ToList();

                        if (issues.Any(i => !issueLabels.Select(l => l.IssueId).Contains(i.IssueId)))
                        {
                            Console.WriteLine($"There are issues which have no labels in \"{option.MileStone}\".");
                            return;
                        }

                        var labels = context.Label
                        .Where(l => l.UserName == option.Owner)
                        .Where(l => l.RepositoryName == option.Repository)
                        .Where(l => issueLabels.Select(i => i.LabelId).Contains(l.LabelId))
                        .ToList();

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
                    }
                });

                var result = CommandLine.Parser.Default.ParseArguments<Options>(args)
                .WithParsed((options) => outputReleasenotes(options));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }

    public class Options
    {
        [Option('o', "owner", Required = true, HelpText = "The owner name of the repository")]
        public string Owner { get; set; }

        [Option('r', "repository", Required = true, HelpText = "The repository name")]
        public string Repository { get; set; }

        [Option('m', "milestone", Required = true, HelpText = "The milestone to publish release notes")]
        public string MileStone { get; set; }
    }
}
