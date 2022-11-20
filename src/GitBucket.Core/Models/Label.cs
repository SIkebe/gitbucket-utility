using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models;

public partial class Label
{
    public string UserName { get; set; } = null!;

    public string RepositoryName { get; set; } = null!;

    public int LabelId { get; set; }

    public string LabelName { get; set; } = null!;

    public string Color { get; set; } = null!;

    public virtual Repository Repository { get; set; } = null!;
}
