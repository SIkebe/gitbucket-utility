using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models;

public partial class Repository
{
    public string UserName { get; set; } = null!;

    public string RepositoryName { get; set; } = null!;

    public bool Private { get; set; }

    public string? Description { get; set; }

    public string? DefaultBranch { get; set; }

    public DateTime RegisteredDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public DateTime LastActivityDate { get; set; }

    public string? OriginUserName { get; set; }

    public string? OriginRepositoryName { get; set; }

    public string? ParentUserName { get; set; }

    public string? ParentRepositoryName { get; set; }

    public string? ExternalIssuesUrl { get; set; }

    public string? ExternalWikiUrl { get; set; }

    public bool? AllowFork { get; set; }

    public string WikiOption { get; set; } = null!;

    public string IssuesOption { get; set; } = null!;

    public string MergeOptions { get; set; } = null!;

    public string DefaultMergeOption { get; set; } = null!;

    public bool? SafeMode { get; set; }

    public virtual ICollection<Collaborator> Collaborators { get; set; } = new List<Collaborator>();

    public virtual ICollection<CommitComment> CommitComments { get; set; } = new List<CommitComment>();

    public virtual ICollection<CommitStatus> CommitStatuses { get; set; } = new List<CommitStatus>();

    public virtual ICollection<CustomField> CustomFields { get; set; } = new List<CustomField>();

    public virtual ICollection<DeployKey> DeployKeys { get; set; } = new List<DeployKey>();

    public virtual IssueId? IssueId { get; set; }

    public virtual ICollection<Issue> Issues { get; set; } = new List<Issue>();

    public virtual ICollection<Label> Labels { get; set; } = new List<Label>();

    public virtual ICollection<Milestone> Milestones { get; set; } = new List<Milestone>();

    public virtual ICollection<Priority> Priorities { get; set; } = new List<Priority>();

    public virtual ICollection<ProtectedBranch> ProtectedBranches { get; set; } = new List<ProtectedBranch>();

    public virtual ICollection<ReleaseTag> ReleaseTags { get; set; } = new List<ReleaseTag>();

    public virtual Account UserNameNavigation { get; set; } = null!;

    public virtual ICollection<WebHook> WebHooks { get; set; } = new List<WebHook>();
}
