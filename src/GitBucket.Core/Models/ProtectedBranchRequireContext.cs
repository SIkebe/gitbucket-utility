namespace GitBucket.Core.Models
{
    public partial class ProtectedBranchRequireContext
    {
        public string UserName { get; set; }
        public string RepositoryName { get; set; }
        public string Branch { get; set; }
        public string Context { get; set; }

        public virtual ProtectedBranch ProtectedBranch { get; set; }
    }
}
