using System;
using System.Collections.Generic;

#nullable disable

namespace GitBucket.Core.Models
{
    public partial class ReleaseTag
    {
        public ReleaseTag()
        {
            ReleaseAssets = new HashSet<ReleaseAsset>();
        }

        public string UserName { get; set; }
        public string RepositoryName { get; set; }
        public string Tag { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public DateTime RegisteredDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public virtual Repository Repository { get; set; }
        public virtual ICollection<ReleaseAsset> ReleaseAssets { get; set; }
    }
}
