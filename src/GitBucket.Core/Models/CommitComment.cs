using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models;

public partial class CommitComment
{
    public string UserName { get; set; } = null!;
    public string RepositoryName { get; set; } = null!;
    public string CommitId { get; set; } = null!;
    public int CommentId { get; set; }
    public string CommentedUserName { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string? FileName { get; set; }
    public int? OldLineNumber { get; set; }
    public int? NewLineNumber { get; set; }
    public DateTime RegisteredDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public int? IssueId { get; set; }
    public string OriginalCommitId { get; set; } = null!;
    public int? OriginalOldLine { get; set; }
    public int? OriginalNewLine { get; set; }

    public virtual Repository Repository { get; set; } = null!;
}
