using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace GbUtil.E2ETests
{
    public class BackupTest : E2ETestBase
    {
        private readonly ITestOutputHelper _output;

        public BackupTest(GitBucketFixture fixture, ITestOutputHelper output) : base(fixture)
        {
            _output = output;
        }

        [Fact]
        public async Task Should_Backup_Repositories()
        {
            // Arrange
            await CreateRepository(autoInit: true);
            await CreateRepository(autoInit: true);

            var gitbucketHome = Path.GetFullPath("../../../../../docker");
            var destination = Path.Combine(Path.GetDirectoryName(Assembly.GetAssembly(typeof(BackupTest))!.Location)!, Guid.NewGuid().ToString());
            var repos = await GitBucketFixture.GitBucketClient.Repository.GetAllForUser(GitBucketDefaults.Owner);

            // Act
            var output = Execute($"backup --home {gitbucketHome} --dest {destination}");
            _output.WriteLine("---------- test output start ----------");
            _output.WriteLine(output);
            _output.WriteLine("---------- test output end ----------");

            // Assert
            foreach (var repo in repos)
            {
                Assert.True(Directory.Exists(Path.Combine(destination, "repositories", GitBucketDefaults.Owner, repo.Name + ".git")));
                Assert.True(Directory.Exists(Path.Combine(destination, "repositories", GitBucketDefaults.Owner, repo.Name + ".wiki.git")));
            }

            Assert.True(Directory.Exists(Path.Combine(destination, "plugins")));
            Assert.True(File.Exists(Path.Combine(destination, "database.conf")));

            Assert.True(output.Contains("Starting clone proces", StringComparison.OrdinalIgnoreCase));
            Assert.True(output.Contains("All repositories cloned", StringComparison.OrdinalIgnoreCase));
            Assert.True(output.Contains("Update repositories: phase 1", StringComparison.OrdinalIgnoreCase));
            Assert.True(output.Contains("Update repositories: phase 1, terminated", StringComparison.OrdinalIgnoreCase));
            Assert.True(output.Contains("Checking pg_dump existence...", StringComparison.OrdinalIgnoreCase));
            Assert.True(output.Contains("Database backup", StringComparison.OrdinalIgnoreCase));
            Assert.True(output.Contains($"The database dump is available in {destination}", StringComparison.OrdinalIgnoreCase));
            Assert.True(output.Contains("Configuration backup", StringComparison.OrdinalIgnoreCase));
            Assert.True(output.Contains("Database configuration backup", StringComparison.OrdinalIgnoreCase));
            Assert.True(output.Contains("Plugins backup", StringComparison.OrdinalIgnoreCase));
            Assert.True(output.Contains("Update repositories: phase 2", StringComparison.OrdinalIgnoreCase));
            Assert.True(output.Contains("Update repositories: phase 2, terminated", StringComparison.OrdinalIgnoreCase));
            Assert.True(output.Contains($"Update process ended, backup available under: {destination}{Path.DirectorySeparatorChar}repositories", StringComparison.OrdinalIgnoreCase));

            var directoryInfo = new DirectoryInfo(destination);
            RemoveReadonlyAttribute(directoryInfo);
            directoryInfo.Delete(recursive: true);
        }
    }
}
