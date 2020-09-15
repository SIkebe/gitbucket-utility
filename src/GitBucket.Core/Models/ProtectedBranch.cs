using System;
using System.Collections.Generic;

#nullable disable

namespace GitBucket.Core.Models
{
    public partial class ProtectedBranch
    {
        public ProtectedBranch()
        {
            ProtectedBranchRequireContexts = new HashSet<ProtectedBranchRequireContext>();
        }

        public string UserName { get; set; }
        public string RepositoryName { get; set; }
        public string Branch { get; set; }
        public bool StatusCheckAdmin { get; set; }

        public virtual Repository Repository { get; set; }
        public virtual ICollection<ProtectedBranchRequireContext> ProtectedBranchRequireContexts { get; set; }
    }
}
