using System;
using System.Collections.Generic;

#nullable disable

namespace GitBucket.Core.Models
{
    public partial class WebHook
    {
        public WebHook()
        {
            WebHookEvents = new HashSet<WebHookEvent>();
        }

        public string UserName { get; set; }
        public string RepositoryName { get; set; }
        public string Url { get; set; }
        public string Token { get; set; }
        public string Ctype { get; set; }

        public virtual Repository Repository { get; set; }
        public virtual ICollection<WebHookEvent> WebHookEvents { get; set; }
    }
}
