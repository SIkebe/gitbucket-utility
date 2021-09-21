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
            var sourceRepositoryName = options.Source.Skip(1).First();
            var destOwner = options.Destination.First();
            var destRepositoryName = options.Destination.Skip(1).First();

            var sourceRepository = await EnsureExists(gitBucketClient, sourceOwner, sourceRepositoryName);
            var destRepository = await EnsureExists(gitBucketClient, destOwner, destRepositoryName);

            switch (options.Type)
            {
                case "move":
                    await MoveIssue(options, gitBucketClient, sourceRepository, destRepository);
                    break;
                case "copy":
                    await CopyIssue(options, gitBucketClient, sourceRepository, destRepository);
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
            Repository sourceRepository,
            Repository destRepository)
        {
            foreach (var issueNumber in options.IssueNumbers)
            {
                var sourceIssue = await gitBucketClient.Issue.Get(
                    sourceRepository.Owner.Login,
                    sourceRepository.Name,
                    issueNumber);

                // Create a new issue on the specified owner/repository
                var newIssue = await gitBucketClient.Issue.Create(
                    destRepository.Owner.Login,
                    destRepository.Name,
                    new NewIssue(sourceIssue.Title)
                    {
                        Body = $"*From @{sourceIssue.User.Login} on {sourceIssue.CreatedAt.LocalDateTime:yyyy-MM-dd HH:mm:ss}*" + Environment.NewLine + Environment.NewLine
                            + sourceIssue.Body + Environment.NewLine + Environment.NewLine
                            + $"*Copied from original issue: {sourceRepository.FullName}#{issueNumber}*"
                    });

                // Copy all labels from the original issue.
                // If there is not the same name label, GitBucket creates one automatically.
                await gitBucketClient.Issue.Labels.AddToIssue(
                    destRepository.Owner.Login,
                    destRepository.Name,
                    newIssue.Number,
                    sourceIssue.Labels.Select(l => l.Name).ToArray());

                // Copy all comments from the original issue
                var issueComments = await gitBucketClient.Issue.Comment.GetAllForIssue(
                    sourceRepository.Owner.Login,
                    sourceRepository.Name,
                    issueNumber);

                foreach (var comment in issueComments)
                {
                    await gitBucketClient.Issue.Comment.Create(
                        destRepository.Owner.Login,
                        destRepository.Name,
                        newIssue.Number,
                        $"*From @{comment.User.Login} on {comment.CreatedAt.LocalDateTime:yyyy-MM-dd HH:mm:ss}*" + Environment.NewLine + Environment.NewLine
                            + comment.Body);
                }

                // Create a comment on the original issue
                await gitBucketClient.Issue.Comment.Create(
                    sourceRepository.Owner.Login,
                    sourceRepository.Name,
                    sourceIssue.Number,
                    $"*This issue was moved to {destRepository.FullName}#{newIssue.Number}*");

                _console.WriteLine($"The issue has been successfully moved to {newIssue.HtmlUrl} .");
                _console.WriteLine($"Close the original one manually.");
            }
        }

        private async Task CopyIssue(
            IssueOptions options,
            IGitHubClient gitBucketClient,
            Repository sourceRepository,
            Repository destRepository)
        {
            foreach (var issueNumber in options.IssueNumbers)
            {
                var sourceIssue = await gitBucketClient.Issue.Get(
                    sourceRepository.Owner.Login,
                    sourceRepository.Name,
                    issueNumber);

                // Create a new issue on the specified owner/repository
                var newIssue = await gitBucketClient.Issue.Create(
                    destRepository.Owner.Login,
                    destRepository.Name,
                    new NewIssue(sourceIssue.Title)
                    {
                        Body = sourceIssue.Body + Environment.NewLine + Environment.NewLine
                            + $"*Copied from original issue: {sourceRepository.FullName}#{issueNumber}*"
                    });

                // Copy all labels from the original issue.
                // If there is not the same name label, GitBucket creates one automatically.
                await gitBucketClient.Issue.Labels.AddToIssue(
                    destRepository.Owner.Login,
                    destRepository.Name,
                    newIssue.Number,
                    sourceIssue.Labels.Select(l => l.Name).ToArray());

                // Copy all comments from the original issue
                var issueComments = await gitBucketClient.Issue.Comment.GetAllForIssue(
                    sourceRepository.Owner.Login,
                    sourceRepository.Name,
                    issueNumber);

                foreach (var comment in issueComments)
                {
                    await gitBucketClient.Issue.Comment.Create(
                        destRepository.Owner.Login,
                        destRepository.Name,
                        newIssue.Number,
                        comment.Body);
                }

                _console.WriteLine($"The issue has been successfully copied to {newIssue.HtmlUrl} .");
            }
        }

        private async Task<Repository> EnsureExists(IGitHubClient gitbucketClient, string owner, string repository)
        {
            try
            {
                return await gitbucketClient.Repository.Get(owner, repository);
            }
            catch (NotFoundException)
            {
                _console.WriteErrorLine($"Repository {owner}/{repository} does not exist.");
                throw;
            }
        }
    }
}
