using System;
using System.Collections.Generic;

#nullable disable

namespace GitBucket.Core.Models
{
    public partial class Account
    {
        public Account()
        {
            AccessTokens = new HashSet<AccessToken>();
            AccountFederations = new HashSet<AccountFederation>();
            AccountWebHooks = new HashSet<AccountWebHook>();
            Collaborators = new HashSet<Collaborator>();
            CommitStatusCreatorNavigations = new HashSet<CommitStatus>();
            CommitStatusUserNameNavigations = new HashSet<CommitStatus>();
            GistComments = new HashSet<GistComment>();
            Gists = new HashSet<Gist>();
            GpgKeys = new HashSet<GpgKey>();
            GroupMemberGroupNameNavigations = new HashSet<GroupMember>();
            GroupMemberUserNameNavigations = new HashSet<GroupMember>();
            Issues = new HashSet<Issue>();
            Repositories = new HashSet<Repository>();
            SshKeys = new HashSet<SshKey>();
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
        public virtual ICollection<AccessToken> AccessTokens { get; set; }
        public virtual ICollection<AccountFederation> AccountFederations { get; set; }
        public virtual ICollection<AccountWebHook> AccountWebHooks { get; set; }
        public virtual ICollection<Collaborator> Collaborators { get; set; }
        public virtual ICollection<CommitStatus> CommitStatusCreatorNavigations { get; set; }
        public virtual ICollection<CommitStatus> CommitStatusUserNameNavigations { get; set; }
        public virtual ICollection<GistComment> GistComments { get; set; }
        public virtual ICollection<Gist> Gists { get; set; }
        public virtual ICollection<GpgKey> GpgKeys { get; set; }
        public virtual ICollection<GroupMember> GroupMemberGroupNameNavigations { get; set; }
        public virtual ICollection<GroupMember> GroupMemberUserNameNavigations { get; set; }
        public virtual ICollection<Issue> Issues { get; set; }
        public virtual ICollection<Repository> Repositories { get; set; }
        public virtual ICollection<SshKey> SshKeys { get; set; }
    }
}
