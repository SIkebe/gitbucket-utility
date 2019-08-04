using System;
using System.Threading.Tasks;
using GitBucket.Core;
using Octokit;
using Octokit.Internal;
using Xunit;

namespace GbUtil.E2ETests
{
    public static class GitBucketDefaults
    {
        public static string Owner { get; set; } = "root";
        public static string Password { get; set; } = "root";
        public static string Repository1 { get; set; } = Guid.NewGuid().ToString();
        public static string Repository2 { get; set; } = Guid.NewGuid().ToString();
    }

    public class GitBucketFixture : IDisposable, IAsyncLifetime
    {
        private bool disposedValue = false;

        public GitBucketFixture()
        {
            GitBucketClient = new GitHubClient(new Connection(
                    new ProductHeaderValue("gbutil"),
                    new Uri("http://localhost:8080/api/v3/"),
                    new InMemoryCredentialStore(new Credentials(GitBucketDefaults.Owner, GitBucketDefaults.Password)),
                    new HttpClientAdapter(() => new GitBucketMessageHandler()),
                    new SimpleJsonSerializer()
                ));
        }

        public IGitHubClient GitBucketClient { get; set; }

        public async Task InitializeAsync()
        {
            await GitBucketClient.Repository.Create(new NewRepository(GitBucketDefaults.Repository1) { AutoInit = true });
            await GitBucketClient.Repository.Create(new NewRepository(GitBucketDefaults.Repository2) { AutoInit = true });
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Called just before Dispose().
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task DisposeAsync()
        {
            // Not Implemented.
            // await GitBucketClient.Repository.Delete(GitBucketDefaults.Owner, GitBucketDefaults.Repository);
            return Task.CompletedTask;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                disposedValue = true;
            }
        }
    }
}
