using System;
using System.Collections.Generic;

namespace gbutil.Models
{
    public partial class CiResult
    {
        public string UserName { get; set; }
        public string RepositoryName { get; set; }
        public string BuildUserName { get; set; }
        public string BuildRepositoryName { get; set; }
        public int BuildNumber { get; set; }
        public string BuildBranch { get; set; }
        public int? PullRequestId { get; set; }
        public string Sha { get; set; }
        public string CommitMessage { get; set; }
        public string CommitUserName { get; set; }
        public string CommitMailAddress { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
        public string BuildAuthor { get; set; }
    }
}
