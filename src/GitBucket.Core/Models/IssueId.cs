using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models;

public partial class IssueId
{
    public string UserName { get; set; } = null!;

    public string RepositoryName { get; set; } = null!;

    public int IssueId1 { get; set; }

    public virtual Repository Repository { get; set; } = null!;
}
