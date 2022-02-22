using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models;

public partial class ProtectedBranchRequireContext
{
    public string UserName { get; set; } = null!;
    public string RepositoryName { get; set; } = null!;
    public string Branch { get; set; } = null!;
    public string Context { get; set; } = null!;

    public virtual ProtectedBranch ProtectedBranch { get; set; } = null!;
}
