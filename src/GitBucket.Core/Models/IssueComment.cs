using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models
{
    public partial class IssueComment
    {
        public string UserName { get; set; }
        public string RepositoryName { get; set; }
        public int IssueId { get; set; }
        public int CommentId { get; set; }
        public string Action { get; set; }
        public string CommentedUserName { get; set; }
        public string Content { get; set; }
        public DateTime RegisteredDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public virtual Issue Issue { get; set; }
    }
}
