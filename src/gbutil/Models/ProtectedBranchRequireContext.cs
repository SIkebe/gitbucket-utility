using System;
using System.Collections.Generic;

namespace gbutil.Models
{
    public partial class ProtectedBranchRequireContext
    {
        public string UserName { get; set; }
        public string RepositoryName { get; set; }
        public string Branch { get; set; }
        public string Context { get; set; }

        public ProtectedBranch ProtectedBranch { get; set; }
    }
}
