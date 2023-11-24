using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models;

public partial class CustomField
{
    public string UserName { get; set; } = null!;

    public string RepositoryName { get; set; } = null!;

    public int FieldId { get; set; }

    public string FieldName { get; set; } = null!;

    public string FieldType { get; set; } = null!;

    public bool EnableForIssues { get; set; }

    public bool EnableForPullRequests { get; set; }

    public string? Constraints { get; set; }

    public virtual ICollection<IssueCustomField> IssueCustomFields { get; set; } = new List<IssueCustomField>();

    public virtual Repository Repository { get; set; } = null!;
}
