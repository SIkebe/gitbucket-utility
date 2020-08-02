using System;
using GitBucket.Core;
using Octokit;
using Octokit.Internal;

namespace GbUtil.E2ETests
{
    public class GitBucketFixture
    {
        public GitBucketFixture()
        {
            Environment.SetEnvironmentVariable("GbUtil_UserName", GitBucketDefaults.Owner);
            Environment.SetEnvironmentVariable("GbUtil_Password", GitBucketDefaults.Password);

            GitBucketClient = new GitHubClient(new Connection(
                    new ProductHeaderValue("gbutil"),
                    new Uri(GitBucketDefaults.ApiEndpoint),
                    new InMemoryCredentialStore(new Credentials(GitBucketDefaults.Owner, GitBucketDefaults.Password)),
                    new HttpClientAdapter(() => new GitBucketMessageHandler()),
                    new SimpleJsonSerializer()
                ));
        }

        public IGitHubClient GitBucketClient { get; set; }
    }
}
