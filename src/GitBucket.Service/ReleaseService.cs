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
        private IConsole _console;

        public ReleaseService(
            IssueRepositoryBase issueRepository,
            LabelRepositoryBase labelRepository,
            IConsole console)
        {
            _issueRepository = issueRepository;
            _labelRepository = labelRepository;
            _console = console;
        }

        public async Task<int> Execute(ReleaseOptions options, IGitHubClient gitBucketClient)
        {
            var closedTargets = options.Target.ToLowerInvariant();
            var issues = await _issueRepository.FindIssuesRelatedToMileStone(options);
            if (!issues.Any())
            {
                _console.WriteWarnLine($"There are no {closedTargets} related to \"{options.MileStone}\".");
                return await Task.FromResult(1);
            }

            if (issues.Any(i => !i.Closed))
            {
                _console.WriteWarnLine($"There are unclosed {closedTargets} in \"{options.MileStone}\".");
                _console.WriteWarn("Do you want to continue?([Y]es/[N]o): ");
                string yesOrNo = Console.ReadLine();

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
                return await CreatePullRequest(options, issues, issueLabels, closedTargets, gitBucketClient);
            }
            else
            {
                return await OutputReleaseNote(options, issues, issueLabels, closedTargets);
            }
        }

        private async Task<int> CreatePullRequest(
            ReleaseOptions options,
            List<Core.Models.Issue> issues,
            List<Core.Models.IssueLabel> issueLabels,
            string closedTargets,
            IGitHubClient gitBucketClient)
        {
            var labels = _labelRepository.FindBy(l =>
                l.UserName.Equals(options.Owner, StringComparison.OrdinalIgnoreCase) &&
                l.RepositoryName.Equals(options.Repository, StringComparison.OrdinalIgnoreCase) &&
                issueLabels.Select(i => i.LabelId).Contains(l.LabelId));

            var highestPriority = issues
                .OrderBy(i => i.Priority.Ordering)
                .First()
                .Priority.PriorityName;

            var builder = new StringBuilder();
            builder.AppendLine($"As part of this release we had {issues.Count} {closedTargets} closed.");
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

            var body = builder.ToString();
            var pr = new NewPullRequest(
                    title: options.Title ?? options.MileStone,
                    head: options.Head,
                    baseRef: options.Base
                ){ Body = body };

            await gitBucketClient.PullRequest.Create(
                options.Owner,
                options.Repository,
                pr);

            return await Task.FromResult(0);
        }

        private async Task<int> OutputReleaseNote(
            ReleaseOptions options,
            List<Core.Models.Issue> issues,
            List<Core.Models.IssueLabel> issueLabels,
            string closedTargets)
        {
            var labels = _labelRepository.FindBy(l =>
                l.UserName.Equals(options.Owner, StringComparison.OrdinalIgnoreCase) &&
                l.RepositoryName.Equals(options.Repository, StringComparison.OrdinalIgnoreCase) &&
                issueLabels.Select(i => i.LabelId).Contains(l.LabelId));

            var highestPriority = issues
                .OrderBy(i => i.Priority.Ordering)
                .First()
                .Priority.PriorityName;

            _console.WriteLine($"As part of this release we had {issues.Count} {closedTargets} closed.");
            _console.WriteLine($"The highest priority among them is \"{highestPriority}\".");
            _console.WriteLine("");
            foreach (var label in labels)
            {
                _console.WriteLine($"### {label.LabelName.ConvertFirstCharToUpper()}");

                var ids = issueLabels
                    .Where(l => l.LabelId == label.LabelId)
                    .Select(i => i.IssueId)
                    .OrderBy(i => i);

                foreach (var issueId in ids)
                {
                    var issue = issues.Where(i => i.IssueId == issueId).Single();
                    _console.WriteLine($"* {issue.Title} #{issue.IssueId}");
                }

                _console.WriteLine("");
            }

            return await Task.FromResult(0);
        }
    }
}
