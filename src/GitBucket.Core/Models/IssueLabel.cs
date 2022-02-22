using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models;

public partial class IssueLabel
{
    public string UserName { get; set; } = null!;
    public string RepositoryName { get; set; } = null!;
    public int IssueId { get; set; }
    public int LabelId { get; set; }

    public virtual Issue Issue { get; set; } = null!;
}
