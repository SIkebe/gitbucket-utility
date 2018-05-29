using System;
using System.Collections.Generic;

namespace gbutil.Models
{
    public partial class PullRequest
    {
        public string UserName { get; set; }
        public string RepositoryName { get; set; }
        public int IssueId { get; set; }
        public string Branch { get; set; }
        public string RequestUserName { get; set; }
        public string RequestRepositoryName { get; set; }
        public string RequestBranch { get; set; }
        public string CommitIdFrom { get; set; }
        public string CommitIdTo { get; set; }
    }
}
