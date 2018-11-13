using System;
using System.Linq;
using System.Threading.Tasks;
using GitBucket.Core;
using GitBucket.Core.Models;
using GitBucket.Data.Repositories;
using Octokit;

namespace GitBucket.Service
{
    public interface IIssueService
    {
        Task<int> MoveIssue(IssueOptions options, IGitHubClient gitBucketClient);
    }

    public class IssueService : IIssueService
    {
        private readonly IConsole _console;

        public IssueService(IConsole console) => _console = console;

        public async Task<int> MoveIssue(IssueOptions options, IGitHubClient gitBucketClient)
        {
            if (options.Type != "move")
            {
                _console.WriteWarnLine($@"""{options.Type}"" is not supported.");
                return 1;
            }

            var source = options.Source.Split('/');
            if (source.Length != 2)
            {
                _console.WriteWarnLine("Incorrect source format.");
                return 1;
            }

            var dest = options.Destination.Split('/');
            if (dest.Length != 2)
            {
                _console.WriteWarnLine("Incorrect destination format.");
                return 1;
            }

            var sourceOwner = source[0];
            var sourceRepository = source[1];
            var destOwner = dest[0];
            var destRepository = dest[1];

            var sourceIssue = await gitBucketClient.Issue.Get(sourceOwner, sourceRepository, options.IssueNumber);

            // Create a new issue on the specified owner/repository
            var newIssue = await gitBucketClient.Issue.Create(
                destOwner,
                destRepository,
                new NewIssue(sourceIssue.Title)
                {
                    Body = $"*From @{sourceIssue.User.Login} on {sourceIssue.CreatedAt.LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss")}*" + Environment.NewLine + Environment.NewLine
                        + sourceIssue.Body + Environment.NewLine + Environment.NewLine
                        + $"*Copied from original issue: {sourceOwner}/{sourceRepository}#{options.IssueNumber}*"
                });

            // Copy all comments from the original issue
            var issueComments = await gitBucketClient.Issue.Comment.GetAllForIssue(sourceOwner, sourceRepository, options.IssueNumber);
            foreach (var comment in issueComments)
            {
                await gitBucketClient.Issue.Comment.Create(
                    destOwner,
                    destRepository,
                    newIssue.Number,
                    $"*From @{comment.User.Login} on {comment.CreatedAt.LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss")}*" + Environment.NewLine + Environment.NewLine
                        + comment.Body);
            }

            // Create a comment on the original issue
            await gitBucketClient.Issue.Comment.Create(
                sourceOwner,
                sourceRepository,
                sourceIssue.Number,
                $"*This issue was moved to {destOwner}/{destRepository}#{newIssue.Number}*");

            _console.WriteLine($"The issue has been successfully moved to {newIssue.HtmlUrl}.");
            _console.WriteLine($"Close the original one manually.");

            return 0;
        }
    }
}