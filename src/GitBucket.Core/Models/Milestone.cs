using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models;

public partial class Milestone
{
    public string UserName { get; set; } = null!;

    public string RepositoryName { get; set; } = null!;

    public int MilestoneId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime? ClosedDate { get; set; }

    public virtual ICollection<Issue> Issues { get; set; } = new List<Issue>();

    public virtual Repository Repository { get; set; } = null!;
}
