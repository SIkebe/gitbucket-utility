using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models;

public partial class IssueAssignee
{
    public string UserName { get; set; } = null!;

    public string RepositoryName { get; set; } = null!;

    public int IssueId { get; set; }

    public string AssigneeUserName { get; set; } = null!;

    public virtual Issue Issue { get; set; } = null!;
}
