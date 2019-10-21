using System;
using System.Threading.Tasks;
using Octokit;

namespace GitBucket.Service.Extensions
{
    public static class IGitHubClientExtension
    {
        public static async Task<IApiResponse<string>> GetCompareHtml(this IGitHubClient client, Repository repository, string @base, string compare)
        {
            if (client is null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            if (repository is null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            var baseAddress = client.Connection.BaseAddress.ToString().Replace(
                client.Connection.BaseAddress.AbsolutePath,
                string.Empty,
                StringComparison.OrdinalIgnoreCase);

            return await client.Connection.GetHtml(new Uri($"{baseAddress}/{repository.Owner.Login}/{repository.Name}/compare/{repository.Owner.Login}:{@base}...{repository.Owner.Login}:{compare}"), null);
        }
    }
}