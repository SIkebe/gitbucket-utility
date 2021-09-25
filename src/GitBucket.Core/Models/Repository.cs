namespace GitBucket.Core.Models;

public partial class Repository
{
    public Repository()
    {
        Collaborators = new HashSet<Collaborator>();
        CommitComments = new HashSet<CommitComment>();
        CommitStatuses = new HashSet<CommitStatus>();
        DeployKeys = new HashSet<DeployKey>();
        Issues = new HashSet<Issue>();
        Labels = new HashSet<Label>();
        Milestones = new HashSet<Milestone>();
        Priorities = new HashSet<Priority>();
        ProtectedBranches = new HashSet<ProtectedBranch>();
        ReleaseTags = new HashSet<ReleaseTag>();
        WebHooks = new HashSet<WebHook>();
    }

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

    public virtual Account UserNameNavigation { get; set; } = null!;
    public virtual IssueId IssueId { get; set; } = null!;
    public virtual ICollection<Collaborator> Collaborators { get; set; }
    public virtual ICollection<CommitComment> CommitComments { get; set; }
    public virtual ICollection<CommitStatus> CommitStatuses { get; set; }
    public virtual ICollection<DeployKey> DeployKeys { get; set; }
    public virtual ICollection<Issue> Issues { get; set; }
    public virtual ICollection<Label> Labels { get; set; }
    public virtual ICollection<Milestone> Milestones { get; set; }
    public virtual ICollection<Priority> Priorities { get; set; }
    public virtual ICollection<ProtectedBranch> ProtectedBranches { get; set; }
    public virtual ICollection<ReleaseTag> ReleaseTags { get; set; }
    public virtual ICollection<WebHook> WebHooks { get; set; }
}
