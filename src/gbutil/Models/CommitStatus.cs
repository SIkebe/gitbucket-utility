using System;
using System.Collections.Generic;

namespace gbutil.Models
{
    public partial class CommitStatus
    {
        public int CommitStatusId { get; set; }
        public string UserName { get; set; }
        public string RepositoryName { get; set; }
        public string CommitId { get; set; }
        public string Context { get; set; }
        public string State { get; set; }
        public string TargetUrl { get; set; }
        public string Description { get; set; }
        public string Creator { get; set; }
        public DateTime RegisteredDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public Account CreatorNavigation { get; set; }
        public Repository Repository { get; set; }
        public Account UserNameNavigation { get; set; }
    }
}
