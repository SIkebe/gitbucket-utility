using System;
using System.Linq;
using System.Threading.Tasks;
using GitBucket.Core;
using GitBucket.Service.Extensions;
using Microsoft.EntityFrameworkCore;
using Octokit;

namespace GitBucket.Service
{
    public interface ICompareService
    {
        Task<int> Execute(CompareOptions options, IGitHubClient gitBucketClient);
    }

    public class CompareService : ICompareService
    {
        private readonly IConsole _console;

        public CompareService(IConsole console)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
        }

        public async Task<int> Execute(CompareOptions options, IGitHubClient gitBucketClient)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (gitBucketClient == null)
            {
                throw new ArgumentNullException(nameof(gitBucketClient));
            }

            foreach (var owner in options.Owners)
            {
                var repositories = await gitBucketClient.Repository.GetAllForOrg(owner);
                foreach (var repository in repositories)
                {
                    var branches = await gitBucketClient.Repository.Branch.GetAll(repository.Owner.Login, repository.Name);
                    var branchNames = branches.Select(b => b.Name);

                    // Compare changes only if the repo has both the target branches.
                    if (branchNames.Contains(options.Compare2) && branchNames.Contains(options.Compare1))
                    {
                        await Compare(gitBucketClient, repository, options.Compare1, options.Compare2);
                        await Compare(gitBucketClient, repository, options.Compare2, options.Compare1);
                    }
                }
            }

            return await Task.FromResult(0);
        }

        private async Task Compare(IGitHubClient gitBucketClient, Repository repository, string @base, string compare)
        {
            var compareResult1 = await gitBucketClient.GetCompareHtml(repository, @base, compare);
            if (!compareResult1.Body.Contains("There isn't anything to compare.", StringComparison.OrdinalIgnoreCase))
            {
                _console.WriteLine($"{compare} in {repository.FullName} has commits which has not merged into {@base}.");
            }
        }
    }
}
