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
            try
            {
                using (var context = new GitBucketDbContext())
                {
                    var issues = await context.Issue
                    .Where(i => i.UserName == "HAP")
                    .Where(i => i.RepositoryName == "HSK")
                    .Where(i => i.Milestone.Title == "v1.3.0")
                    .Where(i => i.Closed == true)
                    .Where(i => !i.PullRequest)
                    .Include(i => i.Milestone)
                    .ToListAsync();

                    if (issues.Any(i => !i.Closed))
                    {
                        Console.WriteLine("Closeされていないissueが存在します。");
                        return;
                    }

                    //var groupedIssues = issues.GroupBy(i=>i.)
                    foreach (var issue in issues)
                    {
                        Console.WriteLine(issue.Title);
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
