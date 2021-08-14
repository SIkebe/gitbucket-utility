using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models
{
    public partial class IssueComment
    {
        public string UserName { get; set; } = null!;
        public string RepositoryName { get; set; } = null!;
        public int IssueId { get; set; }
        public int CommentId { get; set; }
        public string Action { get; set; } = null!;
        public string CommentedUserName { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime RegisteredDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public virtual Issue Issue { get; set; } = null!;
    }
}
