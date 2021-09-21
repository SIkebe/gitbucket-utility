namespace GitBucket.Core.Models
{
    public partial class ReleaseTag
    {
        public ReleaseTag()
        {
            ReleaseAssets = new HashSet<ReleaseAsset>();
        }

        public string UserName { get; set; } = null!;
        public string RepositoryName { get; set; } = null!;
        public string Tag { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Author { get; set; } = null!;
        public string? Content { get; set; }
        public DateTime RegisteredDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public virtual Repository Repository { get; set; } = null!;
        public virtual ICollection<ReleaseAsset> ReleaseAssets { get; set; }
    }
}
