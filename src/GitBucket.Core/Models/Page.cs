using System;
using System.Collections.Generic;

#nullable disable

namespace GitBucket.Core.Models
{
    public partial class Page
    {
        public string UserName { get; set; }
        public string RepositoryName { get; set; }
        public string Source { get; set; }
    }
}
