using System;
using System.Collections.Generic;

#nullable disable

namespace GitBucket.Core.Models
{
    public partial class WebHookBk
    {
        public string UserName { get; set; }
        public string RepositoryName { get; set; }
        public string Url { get; set; }
        public string Token { get; set; }
        public string Ctype { get; set; }
    }
}
