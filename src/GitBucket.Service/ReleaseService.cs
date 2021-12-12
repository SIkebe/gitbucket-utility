using System.Text;
using GitBucket.Core;
using GitBucket.Core.Models;
using Microsoft.EntityFrameworkCore;
using Octokit;

namespace GitBucket.Service;

public interface IReleaseService
{
    Task<int> Execute(ReleaseOptions options, IGitHubClient gitBucketClient);
}

public class ReleaseService : IReleaseService
{
    private readonly DbContext _context;
    private readonly IConsole _console;

    public ReleaseService(DbContext context, IConsole console)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _console = console ?? throw new ArgumentNullException(nameof(console));
    }

    public async Task<int> Execute(ReleaseOptions options, IGitHubClient gitBucketClient)
    {
        if (options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        if (gitBucketClient == null)
        {
            throw new ArgumentNullException(nameof(gitBucketClient));
        }

        var pullRequestSource = options.FromPullRequest ? "pull requests" : "issues";
        var issues = await FindIssuesRelatedToMileStone(options);
        if (!issues.Any())
        {
            _console.WriteWarnLine($"There are no {pullRequestSource} related to \"{options.MileStone}\".");
            return await Task.FromResult(1);
        }

        if (!options.Force && issues.Any(i => !i.Closed))
        {
            _console.WriteWarnLine($"There are unclosed {pullRequestSource} in \"{options.MileStone}\".");
            _console.WriteWarn("Do you want to continue?([Y]es/[N]o): ");
            var yesOrNo = _console.ReadLine();

            if (!string.Equals(yesOrNo, "y", StringComparison.OrdinalIgnoreCase)
            && !string.Equals(yesOrNo, "yes", StringComparison.OrdinalIgnoreCase))
            {
                return await Task.FromResult(1);
            }

            _console.WriteLine("");
        }

        var issueLabels = await FindIssueLabels(options, issues);
        if (issues.Any(i => !issueLabels.Select(l => l.IssueId).Contains(i.IssueId)))
        {
            _console.WriteWarnLine($"There are issues which have no labels in \"{options.MileStone}\".");
            return await Task.FromResult(1);
        }

        var labels = FindLabels(options, issueLabels);
        if (options.CreatePullRequest)
        {
            return await CreatePullRequest(options, issues, issueLabels, pullRequestSource, labels, gitBucketClient);
        }
        else
        {
            return await OutputReleaseNote(issues, issueLabels, pullRequestSource, labels);
        }
    }

    private static string CreateReleaseNote(
        List<Core.Models.Issue> issues,
        List<Core.Models.IssueLabel> issueLabels,
        string pullRequestSource,
        List<Core.Models.Label> labels)
    {
        var highestPriority = issues
            .OrderBy(i => i.Priority?.Ordering)
            .First()
            .Priority?.PriorityName;

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

    private async Task<List<GitBucket.Core.Models.IssueLabel>> FindIssueLabels(
        ReleaseOptions options,
        IEnumerable<GitBucket.Core.Models.Issue> issues)
    {
#pragma warning disable CA1304 // Specify CultureInfo
        // "String.Equals(String, StringComparison)" causes client side evaluation.
        // https://github.com/aspnet/EntityFrameworkCore/issues/1222
        return await _context.Set<GitBucket.Core.Models.IssueLabel>()
            .Where(l => l.UserName.ToLower() == options.Owner.ToLower())
            .Where(l => l.RepositoryName.ToLower() == options.Repository.ToLower())
            .Where(l => issues.Select(i => i.IssueId).Contains(l.IssueId))
            .AsNoTracking()
            .ToListAsync();
    }

    private async Task<List<GitBucket.Core.Models.Issue>> FindIssuesRelatedToMileStone(ReleaseOptions options)
    {
        // "String.Equals(String, StringComparison)" causes client side evaluation.
        // https://github.com/aspnet/EntityFrameworkCore/issues/1222
        return await _context.Set<GitBucket.Core.Models.Issue>()
            .Where(i => i.UserName.ToLower() == options.Owner.ToLower())
            .Where(i => i.RepositoryName.ToLower() == options.Repository.ToLower())
            .Where(i => i.Milestone!.Title.ToLower() == options.MileStone.ToLower())
            .Where(i => i.PullRequest == options.FromPullRequest)
            .Include(i => i.Milestone)
            .Include(i => i.Priority)
            .AsNoTracking()
            .ToListAsync();
#pragma warning restore CA1304 // Specify CultureInfo
    }

    private async Task<int> CreatePullRequest(
        ReleaseOptions options,
        List<Core.Models.Issue> issues,
        List<Core.Models.IssueLabel> issueLabels,
        string pullRequestSource,
        List<Core.Models.Label> labels,
        IGitHubClient gitBucketClient)
    {
        // Check if specified pull request already exists
        var pullRequests = await gitBucketClient.PullRequest.GetAllForRepository(options.Owner, options.Repository);
        if (pullRequests.Any(p => p.Head.Ref == options.Head && p.Base.Ref == options.Base))
        {
            _console.WriteWarnLine($"A pull request already exists for {options.Owner}:{options.Head}.");
            return await Task.FromResult(1);
        }

        var releaseNote = CreateReleaseNote(issues, issueLabels, pullRequestSource, labels);

        try
        {
            // Create new pull request
            await gitBucketClient.PullRequest.Create(
                options.Owner,
                options.Repository,
                new NewPullRequest(
                    title: options.Title ?? options.MileStone,
                    head: options.Head,
                    baseRef: options.Base)
                {
                    Body = releaseNote,
                    Draft = options.Draft,
                });
        }
        catch (InvalidCastException)
        {
            // Ignore InvalidCastException because of escaped response.
            // https://github.com/gitbucket/gitbucket/issues/2306
        }

        var allPRs = await gitBucketClient.PullRequest.GetAllForRepository(options.Owner, options.Repository);
        var latest = allPRs.OrderByDescending(p => p.Number).First();

        // Add all labels which corresponding issues have.
        // If there is not the same name label, GitBucket creates one automatically.
        await gitBucketClient.Issue.Labels.AddToIssue(
            options.Owner,
            options.Repository,
            latest.Number,
            labels.Select(l => l.LabelName).ToArray());

        _console.WriteLine($"A new pull request has been successfully created!");
        return await Task.FromResult(0);
    }

    private async Task<int> OutputReleaseNote(
        List<Core.Models.Issue> issues,
        List<Core.Models.IssueLabel> issueLabels,
        string pullRequestSource,
        List<Core.Models.Label> labels)
    {
        var releaseNote = CreateReleaseNote(issues, issueLabels, pullRequestSource, labels);
        _console.WriteLine(releaseNote);
        return await Task.FromResult(0);
    }

    private List<Core.Models.Label> FindLabels(ReleaseOptions options, List<IssueLabel> issueLabels)
    {
#pragma warning disable CA1304 // Specify CultureInfo

        return _context.Set<Core.Models.Label>()
            .Where(l =>
                l.UserName.ToLower() == options.Owner.ToLower() &&
                l.RepositoryName.ToLower() == options.Repository.ToLower() &&
                issueLabels.Select(i => i.LabelId).Contains(l.LabelId))
            .OrderBy(i => i.LabelId)
            .AsNoTracking()
            .ToList();

#pragma warning restore CA1304 // Specify CultureInfo
    }
}
