using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Octokit;
using Xunit;

namespace GbUtil.E2ETests
{
    public abstract class E2ETestBase : IClassFixture<GitBucketFixture>
    {
        public E2ETestBase(GitBucketFixture fixture)
        {
            GitBucketFixture = fixture;
        }

        public static string GbUtilDll
        {
            get
            {
                var gbUtilE2ETestsDll = Assembly.GetAssembly(typeof(E2ETestBase))!.Location;
                var gbUtilDll = gbUtilE2ETestsDll.Replace(".E2ETests", string.Empty, StringComparison.OrdinalIgnoreCase);
                return gbUtilDll;
            }
        }

        public GitBucketFixture GitBucketFixture { get; set; }

        public async Task<Repository> CreateRepository(bool autoInit = false)
        {
            var repository = Guid.NewGuid().ToString();
            return await GitBucketFixture.GitBucketClient.Repository.Create(new NewRepository(repository) { AutoInit = autoInit });
        }

        protected static string Execute(string arguments)
        {
            using var process = new Process();
            process.StartInfo.FileName = "dotnet";
            process.StartInfo.Arguments = $@"""{GbUtilDll}"" {arguments}";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;

            process.Start();

            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output;
        }
    }
}
