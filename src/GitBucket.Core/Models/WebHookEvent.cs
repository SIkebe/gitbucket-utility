using System;
using System.Collections.Generic;

#nullable disable

namespace GitBucket.Core.Models
{
    public partial class WebHookEvent
    {
        public string UserName { get; set; }
        public string RepositoryName { get; set; }
        public string Url { get; set; }
        public string Event { get; set; }

        public virtual WebHook WebHook { get; set; }
    }
}
