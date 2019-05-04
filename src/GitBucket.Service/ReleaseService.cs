using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GitBucket.Core;
using GitBucket.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Octokit;

namespace GitBucket.Service
{
    public interface IReleaseService
    {
        Task<int> Execute(ReleaseOptions options, IGitHubClient gitBucketClient);
    }

    public class ReleaseService : IReleaseService
    {
        private readonly IssueRepositoryBase _issueRepository;
        private readonly LabelRepositoryBase _labelRepository;
        private readonly IConsole _console;

        public ReleaseService(
            IssueRepositoryBase issueRepository,
            LabelRepositoryBase labelRepository,
            IConsole console)
        {
            _issueRepository = issueRepository ?? throw new ArgumentNullException(nameof(issueRepository));
            _labelRepository = labelRepository ?? throw new ArgumentNullException(nameof(labelRepository));
            _console = console ?? throw new ArgumentNullException(nameof(console));
        }

        public async Task<int> Execute(ReleaseOptions options, IGitHubClient gitBucketClient)
        {
            var pullRequestSource = options.FromPullRequest ? "pull requests" : "issues";
            var issues = await _issueRepository.FindIssuesRelatedToMileStone(options);
            if (!issues.Any())
            {
                _console.WriteWarnLine($"There are no {pullRequestSource} related to \"{options.MileStone}\".");
                return await Task.FromResult(1);
            }

            if (issues.Any(i => !i.Closed))
            {
                _console.WriteWarnLine($"There are unclosed {pullRequestSource} in \"{options.MileStone}\".");
                _console.WriteWarn("Do you want to continue?([Y]es/[N]o): ");
                string yesOrNo = _console.ReadLine();

                if (!string.Equals(yesOrNo, "y", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(yesOrNo, "yes", StringComparison.OrdinalIgnoreCase))
                {
                    return await Task.FromResult(1);
                }

                _console.WriteLine("");
            }

            var issueLabels = _issueRepository.FindIssueLabels(options, issues).ToList();
            if (issues.Any(i => !issueLabels.Select(l => l.IssueId).Contains(i.IssueId)))
            {
                _console.WriteWarnLine($"There are issues which have no labels in \"{options.MileStone}\".");
                return await Task.FromResult(1);
            }

            if (options.CreatePullRequest)
            {
                return await CreatePullRequest(options, issues, issueLabels, pullRequestSource, gitBucketClient);
            }
            else
            {
                return await OutputReleaseNote(options, issues, issueLabels, pullRequestSource);
            }
        }

        private async Task<int> CreatePullRequest(
            ReleaseOptions options,
            List<Core.Models.Issue> issues,
            List<Core.Models.IssueLabel> issueLabels,
            string pullRequestSource,
            IGitHubClient gitBucketClient)
        {
            // Check if specified pull request already exists
            var pullRequests = await gitBucketClient.PullRequest.GetAllForRepository(options.Owner, options.Repository);
            if (pullRequests.Any(p => p.Head.Ref == options.Head && p.Base.Ref == options.Base))
            {
                _console.WriteWarnLine($"A pull request already exists for {options.Owner}:{options.Head}.");
                return await Task.FromResult(1);
            }

            var releaseNote = CreateReleaseNote(options, issues, issueLabels, pullRequestSource);

            try
            {
                // Create new pull request
                await gitBucketClient.PullRequest.Create(
                    options.Owner,
                    options.Repository,
                    new NewPullRequest(
                        title: options.Title ?? options.MileStone,
                        head: options.Head,
                        baseRef: options.Base
                    )
                    { Body = releaseNote });
            }
            catch (InvalidCastException)
            {
                // Ignore InvalidCastException because of escaped response.
                // https://github.com/gitbucket/gitbucket/issues/2306
            }

            _console.WriteLine($"A new pull request has been successfully created!");
            return await Task.FromResult(0);
        }

        private async Task<int> OutputReleaseNote(
            ReleaseOptions options,
            List<Core.Models.Issue> issues,
            List<Core.Models.IssueLabel> issueLabels,
            string pullRequestSource)
        {
            var releaseNote = CreateReleaseNote(options, issues, issueLabels, pullRequestSource);
            _console.WriteLine(releaseNote);
            return await Task.FromResult(0);
        }

        private string CreateReleaseNote(
            ReleaseOptions options,
            List<Core.Models.Issue> issues,
            List<Core.Models.IssueLabel> issueLabels,
            string pullRequestSource)
        {
            var labels = _labelRepository
                .FindBy(l =>
                    l.UserName.Equals(options.Owner, StringComparison.OrdinalIgnoreCase) &&
                    l.RepositoryName.Equals(options.Repository, StringComparison.OrdinalIgnoreCase) &&
                    issueLabels.Select(i => i.LabelId).Contains(l.LabelId));

            var highestPriority = issues
                .OrderBy(i => i.Priority.Ordering)
                .First()
                .Priority.PriorityName;

            var builder = new StringBuilder();
            builder.AppendLine($"As part of this release we had {issues.Count} {pullRequestSource} closed.");
            builder.AppendLine($"The highest priority among them is \"{highestPriority}\".");
            builder.AppendLine("");
            foreach (var label in labels)
            {
                builder.AppendLine($"### {label.LabelName.ConvertFirstCharToUpper()}");

                var ids = issueLabels
                    .Where(l => l.LabelId == label.LabelId)
                    .Select(i => i.IssueId)
                    .OrderBy(i => i);

                foreach (var issueId in ids)
                {
                    var issue = issues.Where(i => i.IssueId == issueId).Single();
                    builder.AppendLine($"* {issue.Title} #{issue.IssueId}");
                }

                builder.AppendLine("");
            }

            return builder.ToString();
        }
    }
}
