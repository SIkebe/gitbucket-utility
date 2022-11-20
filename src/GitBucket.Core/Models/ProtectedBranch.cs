using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models;

public partial class ProtectedBranch
{
    public string UserName { get; set; } = null!;

    public string RepositoryName { get; set; } = null!;

    public string Branch { get; set; } = null!;

    public bool StatusCheckAdmin { get; set; }

    public virtual ICollection<ProtectedBranchRequireContext> ProtectedBranchRequireContexts { get; } = new List<ProtectedBranchRequireContext>();

    public virtual Repository Repository { get; set; } = null!;
}
