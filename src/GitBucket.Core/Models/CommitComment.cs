using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models
{
    public partial class CommitComment
    {
        public string UserName { get; set; }
        public string RepositoryName { get; set; }
        public string CommitId { get; set; }
        public int CommentId { get; set; }
        public string CommentedUserName { get; set; }
        public string Content { get; set; }
        public string FileName { get; set; }
        public int? OldLineNumber { get; set; }
        public int? NewLineNumber { get; set; }
        public DateTime RegisteredDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int? IssueId { get; set; }
        public string OriginalCommitId { get; set; }
        public int? OriginalOldLine { get; set; }
        public int? OriginalNewLine { get; set; }

        public Repository Repository { get; set; }
    }
}
