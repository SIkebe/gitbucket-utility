using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models
{
    public partial class DeployKey
    {
        public string UserName { get; set; } = null!;
        public string RepositoryName { get; set; } = null!;
        public int DeployKeyId { get; set; }
        public string Title { get; set; } = null!;
        public string PublicKey { get; set; } = null!;
        public bool AllowWrite { get; set; }

        public virtual Repository Repository { get; set; } = null!;
    }
}
