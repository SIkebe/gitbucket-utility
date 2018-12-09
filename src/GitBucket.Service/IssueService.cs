using System;
using System.Linq;
using System.Threading.Tasks;
using GitBucket.Core;
using Octokit;

namespace GitBucket.Service
{
    public interface IIssueService
    {
        Task<int> Execute(IssueOptions options, IGitHubClient gitBucketClient);
    }

    public class IssueService : IIssueService
    {
        private readonly IConsole _console;

        public IssueService(IConsole console) => _console = console;

        public async Task<int> Execute(IssueOptions options, IGitHubClient gitBucketClient)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            if (gitBucketClient == null) throw new ArgumentNullException(nameof(gitBucketClient));

            var sourceOwner = options.Source.First();
            var sourceRepository = options.Source.Skip(1).First();
            var destOwner = options.Destination.First();
            var destRepository = options.Destination.Skip(1).First();

            switch (options.Type)
            {
                case "move":
                    await MoveIssue(options, gitBucketClient, sourceOwner, sourceRepository, destOwner, destRepository);
                    break;
                case "copy":
                    await CopyIssue(options, gitBucketClient, sourceOwner, sourceRepository, destOwner, destRepository);
                    break;
                default:
                    _console.WriteWarnLine($@"""{options.Type}"" is not supported.");
                    return 1;
            }

            return 0;
        }

        private async Task MoveIssue(
            IssueOptions options,
            IGitHubClient gitBucketClient,
            string sourceOwner,
            string sourceRepository,
            string destOwner,
            string destRepository)
        {
            foreach (var issueNumber in options.IssueNumbers)
            {
                var sourceIssue = await gitBucketClient.Issue.Get(sourceOwner, sourceRepository, issueNumber);

                // Create a new issue on the specified owner/repository
                var newIssue = await gitBucketClient.Issue.Create(
                    destOwner,
                    destRepository,
                    new NewIssue(sourceIssue.Title)
                    {
                        Body = $"*From @{sourceIssue.User.Login} on {sourceIssue.CreatedAt.LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss")}*" + Environment.NewLine + Environment.NewLine
                            + sourceIssue.Body + Environment.NewLine + Environment.NewLine
                            + $"*Copied from original issue: {sourceOwner}/{sourceRepository}#{issueNumber}*"
                    });

                // Copy all comments from the original issue
                var issueComments = await gitBucketClient.Issue.Comment.GetAllForIssue(sourceOwner, sourceRepository, issueNumber);
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
            }
        }

        private async Task CopyIssue(
            IssueOptions options,
            IGitHubClient gitBucketClient,
            string sourceOwner,
            string sourceRepository,
            string destOwner,
            string destRepository)
        {
            foreach (var issueNumber in options.IssueNumbers)
            {
                var sourceIssue = await gitBucketClient.Issue.Get(sourceOwner, sourceRepository, issueNumber);

                // Create a new issue on the specified owner/repository
                var newIssue = await gitBucketClient.Issue.Create(
                    destOwner,
                    destRepository,
                    new NewIssue(sourceIssue.Title)
                    {
                        Body = sourceIssue.Body + Environment.NewLine + Environment.NewLine
                            + $"*Copied from original issue: {sourceOwner}/{sourceRepository}#{issueNumber}*"
                    });

                // Copy all comments from the original issue
                var issueComments = await gitBucketClient.Issue.Comment.GetAllForIssue(sourceOwner, sourceRepository, issueNumber);
                foreach (var comment in issueComments)
                {
                    await gitBucketClient.Issue.Comment.Create(destOwner, destRepository, newIssue.Number, comment.Body);
                }

                _console.WriteLine($"The issue has been successfully copied to {newIssue.HtmlUrl}.");
            }
        }
    }
}