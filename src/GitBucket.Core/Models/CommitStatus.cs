using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models;

public partial class CommitStatus
{
    public int CommitStatusId { get; set; }

    public string UserName { get; set; } = null!;

    public string RepositoryName { get; set; } = null!;

    public string CommitId { get; set; } = null!;

    public string Context { get; set; } = null!;

    public string State { get; set; } = null!;

    public string? TargetUrl { get; set; }

    public string? Description { get; set; }

    public string Creator { get; set; } = null!;

    public DateTime RegisteredDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public virtual Account CreatorNavigation { get; set; } = null!;

    public virtual Repository Repository { get; set; } = null!;

    public virtual Account UserNameNavigation { get; set; } = null!;
}
