using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models
{
    public partial class CiConfig
    {
        public string UserName { get; set; }
        public string RepositoryName { get; set; }
        public string BuildScript { get; set; }
        public bool Notification { get; set; }
        public string SkipWords { get; set; }
        public string RunWords { get; set; }
    }
}
