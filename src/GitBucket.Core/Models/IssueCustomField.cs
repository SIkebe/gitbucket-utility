using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models;

public partial class IssueCustomField
{
    public string UserName { get; set; } = null!;

    public string RepositoryName { get; set; } = null!;

    public int IssueId { get; set; }

    public int FieldId { get; set; }

    public string? Value { get; set; }

    public virtual CustomField CustomField { get; set; } = null!;

    public virtual Issue Issue { get; set; } = null!;
}
