using System;
using System.Threading.Tasks;
using Octokit;

namespace gbutil
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var client = new GitHubClient(
                    new ProductHeaderValue("test"),
                    new Uri("http://localhost:8080/api/v3/"));

                var openIssues = await client.Issue.GetAllForRepository(
                    "root",
                    "test",
                    new RepositoryIssueRequest { State = ItemStateFilter.Open },
                    ApiOptions.None);

                Console.WriteLine("Open issues");
                foreach (var issue in openIssues)
                {
                    Console.WriteLine(issue.Title);
                }

                Console.WriteLine("");

                var closedIssues = await client.Issue.GetAllForRepository(
                    "root",
                    "test",
                    new RepositoryIssueRequest { State = ItemStateFilter.Closed },
                    ApiOptions.None);

                Console.WriteLine("Closed issues");
                foreach (var issue in closedIssues)
                {
                    Console.WriteLine(issue.Title);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
