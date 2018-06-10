using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models
{
    public partial class Repository
    {
        public Repository()
        {
            Activity = new HashSet<Activity>();
            Collaborator = new HashSet<Collaborator>();
            CommitComment = new HashSet<CommitComment>();
            CommitStatus = new HashSet<CommitStatus>();
            DeployKey = new HashSet<DeployKey>();
            Issue = new HashSet<Issue>();
            Label = new HashSet<Label>();
            Milestone = new HashSet<Milestone>();
            Priority = new HashSet<Priority>();
            ProtectedBranch = new HashSet<ProtectedBranch>();
            ReleaseTag = new HashSet<ReleaseTag>();
            WebHook = new HashSet<WebHook>();
        }

        public string UserName { get; set; }
        public string RepositoryName { get; set; }
        public bool Private { get; set; }
        public string Description { get; set; }
        public string DefaultBranch { get; set; }
        public DateTime RegisteredDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime LastActivityDate { get; set; }
        public string OriginUserName { get; set; }
        public string OriginRepositoryName { get; set; }
        public string ParentUserName { get; set; }
        public string ParentRepositoryName { get; set; }
        public string ExternalIssuesUrl { get; set; }
        public string ExternalWikiUrl { get; set; }
        public bool? AllowFork { get; set; }
        public string WikiOption { get; set; }
        public string IssuesOption { get; set; }
        public string MergeOptions { get; set; }
        public string DefaultMergeOption { get; set; }

        public Account UserNameNavigation { get; set; }
        public IssueId IssueId { get; set; }
        public ICollection<Activity> Activity { get; set; }
        public ICollection<Collaborator> Collaborator { get; set; }
        public ICollection<CommitComment> CommitComment { get; set; }
        public ICollection<CommitStatus> CommitStatus { get; set; }
        public ICollection<DeployKey> DeployKey { get; set; }
        public ICollection<Issue> Issue { get; set; }
        public ICollection<Label> Label { get; set; }
        public ICollection<Milestone> Milestone { get; set; }
        public ICollection<Priority> Priority { get; set; }
        public ICollection<ProtectedBranch> ProtectedBranch { get; set; }
        public ICollection<ReleaseTag> ReleaseTag { get; set; }
        public ICollection<WebHook> WebHook { get; set; }
    }
}
