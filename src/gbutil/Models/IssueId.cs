using System;
using System.Collections.Generic;

namespace gbutil.Models
{
    public partial class IssueId
    {
        public string UserName { get; set; }
        public string RepositoryName { get; set; }
        public int IssueId1 { get; set; }

        public Repository Repository { get; set; }
    }
}
