using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace GitBucket.Core
{
    public class GitBucketMessageHandler : DelegatingHandler
    {
        public GitBucketMessageHandler() : base(new HttpClientHandler())
        {
        }

        protected async override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken = default)
        {
            if (request != null)
            {
                if (request.Content != null)
                {
                    var contentType = request.Content.Headers.ContentType.MediaType;
                    if (contentType == "application/x-www-form-urlencoded")
                    {
                        // GitBucket doesn't accept Content-Type: application/x-www-form-urlencoded
                        request.Content.Headers.ContentType.MediaType = "application/json";
                    }
                }
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
