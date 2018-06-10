using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models
{
    public partial class ProtectedBranch
    {
        public ProtectedBranch()
        {
            ProtectedBranchRequireContext = new HashSet<ProtectedBranchRequireContext>();
        }

        public string UserName { get; set; }
        public string RepositoryName { get; set; }
        public string Branch { get; set; }
        public bool? StatusCheckAdmin { get; set; }

        public Repository Repository { get; set; }
        public ICollection<ProtectedBranchRequireContext> ProtectedBranchRequireContext { get; set; }
    }
}
