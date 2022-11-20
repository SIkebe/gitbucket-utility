using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models;

public partial class Issue
{
    public string UserName { get; set; } = null!;

    public string RepositoryName { get; set; } = null!;

    public int IssueId { get; set; }

    public string OpenedUserName { get; set; } = null!;

    public int? MilestoneId { get; set; }

    public string? AssignedUserName { get; set; }

    public string Title { get; set; } = null!;

    public string? Content { get; set; }

    public bool Closed { get; set; }

    public DateTime RegisteredDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public bool PullRequest { get; set; }

    public int? PriorityId { get; set; }

    public virtual ICollection<IssueComment> IssueComments { get; } = new List<IssueComment>();

    public virtual ICollection<IssueLabel> IssueLabels { get; } = new List<IssueLabel>();

    public virtual Milestone? Milestone { get; set; }

    public virtual Account OpenedUserNameNavigation { get; set; } = null!;

    public virtual Priority? Priority { get; set; }

    public virtual PullRequest? PullRequestNavigation { get; set; }

    public virtual Repository Repository { get; set; } = null!;
}
