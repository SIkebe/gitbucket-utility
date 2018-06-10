using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models
{
    public partial class ReleaseTag
    {
        public ReleaseTag()
        {
            ReleaseAsset = new HashSet<ReleaseAsset>();
        }

        public string UserName { get; set; }
        public string RepositoryName { get; set; }
        public string Tag { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public DateTime RegisteredDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public Repository Repository { get; set; }
        public ICollection<ReleaseAsset> ReleaseAsset { get; set; }
    }
}
