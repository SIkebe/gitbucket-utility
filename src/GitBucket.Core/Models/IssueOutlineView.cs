using System;
using System.Collections.Generic;

#nullable disable

namespace GitBucket.Core.Models
{
    public partial class IssueOutlineView
    {
        public string UserName { get; set; }
        public string RepositoryName { get; set; }
        public int? IssueId { get; set; }
        public long? CommentCount { get; set; }
        public int? Priority { get; set; }
    }
}
