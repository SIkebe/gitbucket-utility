using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models
{
    public partial class WebHookEvent
    {
        public string UserName { get; set; }
        public string RepositoryName { get; set; }
        public string Url { get; set; }
        public string Event { get; set; }

        public WebHook WebHook { get; set; }
    }
}
