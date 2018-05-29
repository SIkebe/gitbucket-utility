using System;
using System.Collections.Generic;

namespace gbutil.Models
{
    public partial class DeployKey
    {
        public string UserName { get; set; }
        public string RepositoryName { get; set; }
        public int DeployKeyId { get; set; }
        public string Title { get; set; }
        public string PublicKey { get; set; }
        public bool? AllowWrite { get; set; }

        public Repository Repository { get; set; }
    }
}
