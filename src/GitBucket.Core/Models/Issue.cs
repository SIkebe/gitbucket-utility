using System;
using System.Collections.Generic;

#nullable disable

namespace GitBucket.Core.Models
{
    public partial class Issue
    {
        public Issue()
        {
            IssueComments = new HashSet<IssueComment>();
            IssueLabels = new HashSet<IssueLabel>();
        }

        public string UserName { get; set; }
        public string RepositoryName { get; set; }
        public int IssueId { get; set; }
        public string OpenedUserName { get; set; }
        public int? MilestoneId { get; set; }
        public string AssignedUserName { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool Closed { get; set; }
        public DateTime RegisteredDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool PullRequest { get; set; }
        public int? PriorityId { get; set; }

        public virtual Milestone Milestone { get; set; }
        public virtual Account OpenedUserNameNavigation { get; set; }
        public virtual Priority Priority { get; set; }
        public virtual Repository Repository { get; set; }
        public virtual PullRequest PullRequestNavigation { get; set; }
        public virtual ICollection<IssueComment> IssueComments { get; set; }
        public virtual ICollection<IssueLabel> IssueLabels { get; set; }
    }
}
