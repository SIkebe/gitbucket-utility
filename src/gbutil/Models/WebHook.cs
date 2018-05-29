using System;
using System.Collections.Generic;

namespace gbutil.Models
{
    public partial class WebHook
    {
        public WebHook()
        {
            WebHookEvent = new HashSet<WebHookEvent>();
        }

        public string UserName { get; set; }
        public string RepositoryName { get; set; }
        public string Url { get; set; }
        public string Token { get; set; }
        public string Ctype { get; set; }

        public Repository Repository { get; set; }
        public ICollection<WebHookEvent> WebHookEvent { get; set; }
    }
}
