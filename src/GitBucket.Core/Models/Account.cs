using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models;

public partial class Account
{
    public string UserName { get; set; } = null!;

    public string MailAddress { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool Administrator { get; set; }

    public string? Url { get; set; }

    public DateTime RegisteredDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public DateTime? LastLoginDate { get; set; }

    public string? Image { get; set; }

    public bool GroupAccount { get; set; }

    public string FullName { get; set; } = null!;

    public bool? Removed { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<AccessToken> AccessTokens { get; } = new List<AccessToken>();

    public virtual ICollection<AccountFederation> AccountFederations { get; } = new List<AccountFederation>();

    public virtual AccountPreference? AccountPreference { get; set; }

    public virtual ICollection<AccountWebHook> AccountWebHooks { get; } = new List<AccountWebHook>();

    public virtual ICollection<Collaborator> Collaborators { get; } = new List<Collaborator>();

    public virtual ICollection<CommitStatus> CommitStatusCreatorNavigations { get; } = new List<CommitStatus>();

    public virtual ICollection<CommitStatus> CommitStatusUserNameNavigations { get; } = new List<CommitStatus>();

    public virtual ICollection<GistComment> GistComments { get; } = new List<GistComment>();

    public virtual ICollection<Gist> Gists { get; } = new List<Gist>();

    public virtual ICollection<GpgKey> GpgKeys { get; } = new List<GpgKey>();

    public virtual ICollection<GroupMember> GroupMemberGroupNameNavigations { get; } = new List<GroupMember>();

    public virtual ICollection<GroupMember> GroupMemberUserNameNavigations { get; } = new List<GroupMember>();

    public virtual ICollection<Issue> Issues { get; } = new List<Issue>();

    public virtual NotificationsAccount? NotificationsAccount { get; set; }

    public virtual ICollection<Repository> Repositories { get; } = new List<Repository>();

    public virtual ICollection<SshKey> SshKeys { get; } = new List<SshKey>();
}
