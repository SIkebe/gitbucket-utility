using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models
{
    public partial class Account
    {
        public Account()
        {
            AccessToken = new HashSet<AccessToken>();
            AccountFederation = new HashSet<AccountFederation>();
            AccountWebHook = new HashSet<AccountWebHook>();
            Activity = new HashSet<Activity>();
            Collaborator = new HashSet<Collaborator>();
            CommitStatusCreatorNavigation = new HashSet<CommitStatus>();
            CommitStatusUserNameNavigation = new HashSet<CommitStatus>();
            Gist = new HashSet<Gist>();
            GistComment = new HashSet<GistComment>();
            GroupMemberGroupNameNavigation = new HashSet<GroupMember>();
            GroupMemberUserNameNavigation = new HashSet<GroupMember>();
            Issue = new HashSet<Issue>();
            Repository = new HashSet<Repository>();
            SshKey = new HashSet<SshKey>();
        }

        public string UserName { get; set; }
        public string MailAddress { get; set; }
        public string Password { get; set; }
        public bool Administrator { get; set; }
        public string Url { get; set; }
        public DateTime RegisteredDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public string Image { get; set; }
        public bool GroupAccount { get; set; }
        public string FullName { get; set; }
        public bool? Removed { get; set; }
        public string Description { get; set; }

        public virtual NotificationsAccount NotificationsAccount { get; set; }
        public virtual ICollection<AccessToken> AccessToken { get; set; }
        public virtual ICollection<AccountFederation> AccountFederation { get; set; }
        public virtual ICollection<AccountWebHook> AccountWebHook { get; set; }
        public virtual ICollection<Activity> Activity { get; set; }
        public virtual ICollection<Collaborator> Collaborator { get; set; }
        public virtual ICollection<CommitStatus> CommitStatusCreatorNavigation { get; set; }
        public virtual ICollection<CommitStatus> CommitStatusUserNameNavigation { get; set; }
        public virtual ICollection<Gist> Gist { get; set; }
        public virtual ICollection<GistComment> GistComment { get; set; }
        public virtual ICollection<GroupMember> GroupMemberGroupNameNavigation { get; set; }
        public virtual ICollection<GroupMember> GroupMemberUserNameNavigation { get; set; }
        public virtual ICollection<Issue> Issue { get; set; }
        public virtual ICollection<Repository> Repository { get; set; }
        public virtual ICollection<SshKey> SshKey { get; set; }
    }
}
