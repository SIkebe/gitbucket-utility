using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models
{
    public partial class PullRequest
    {
        public string UserName { get; set; } = null!;
        public string RepositoryName { get; set; } = null!;
        public int IssueId { get; set; }
        public string Branch { get; set; } = null!;
        public string RequestUserName { get; set; } = null!;
        public string RequestRepositoryName { get; set; } = null!;
        public string RequestBranch { get; set; } = null!;
        public string CommitIdFrom { get; set; } = null!;
        public string CommitIdTo { get; set; } = null!;
        public bool IsDraft { get; set; }

        public virtual Issue Issue { get; set; } = null!;
    }
}
