using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models
{
    public partial class IssueId
    {
        public string UserName { get; set; }
        public string RepositoryName { get; set; }
        public int IssueId1 { get; set; }

        public Repository Repository { get; set; }
    }
}
