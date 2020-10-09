using System;
using System.Collections.Generic;

#nullable disable

namespace GitBucket.Core.Models
{
    public partial class IssueId
    {
        public string UserName { get; set; }
        public string RepositoryName { get; set; }
        public int IssueId1 { get; set; }

        public virtual Repository Repository { get; set; }
    }
}
