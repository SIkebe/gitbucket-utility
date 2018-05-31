using System;
using System.Linq;
using System.Threading.Tasks;
using gbutil.Models;
using Microsoft.EntityFrameworkCore;
using Octokit;

namespace gbutil
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var owner = "owner";
            var repositoryName = "repositoryName";
            var mileStoneTitle = "mileStoneTitle";

            try
            {
                using (var context = new GitBucketDbContext())
                {
                    var issues = await context.Issue
                    .Where(i => i.UserName == owner)
                    .Where(i => i.RepositoryName == repositoryName)
                    .Where(i => i.Milestone.Title == mileStoneTitle)
                    .Where(i => i.Closed == true)
                    .Where(i => !i.PullRequest)
                    .Include(i => i.Milestone)
                    .ToListAsync();

                    if (issues.Count == 0)
                    {
                        Console.WriteLine($"There are no issues related to \"{mileStoneTitle}\".");
                        return;
                    }

                    if (issues.Any(i => !i.Closed))
                    {
                        Console.WriteLine($"There are unclosed issues in \"{mileStoneTitle}\".");
                        return;
                    }

                    var issueLabels = await context.IssueLabel
                    .Where(l => l.UserName == owner)
                    .Where(l => l.RepositoryName == repositoryName)
                    .Where(l => issues.Select(i => i.IssueId).Contains(l.IssueId))
                    .ToListAsync();

                    if (issues.Any(i => !issueLabels.Select(l => l.IssueId).Contains(i.IssueId)))
                    {
                        Console.WriteLine($"There are issues which have no labels in \"{mileStoneTitle}\".");
                        return;
                    }

                    var labels = await context.Label
                    .Where(l => l.UserName == owner)
                    .Where(l => l.RepositoryName == repositoryName)
                    .Where(l => issueLabels.Select(i => i.LabelId).Contains(l.LabelId))
                    .ToListAsync();

                    foreach (var label in labels)
                    {
                        Console.WriteLine($"###{label.LabelName}");
                        var ids = issueLabels.Where(l => l.LabelId == label.LabelId).Select(i => i.IssueId).OrderBy(i => i);

                        foreach (var issueId in ids)
                        {
                            var issue = issues.Where(i => i.IssueId == issueId).Single();
                            Console.WriteLine($"* {issue.Title} #{issue.IssueId}");
                        }

                        Console.WriteLine("");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
