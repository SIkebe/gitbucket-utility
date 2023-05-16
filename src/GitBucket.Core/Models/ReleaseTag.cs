using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models;

public partial class ReleaseTag
{
    public string UserName { get; set; } = null!;

    public string RepositoryName { get; set; } = null!;

    public string Tag { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Author { get; set; } = null!;

    public string? Content { get; set; }

    public DateTime RegisteredDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public virtual ICollection<ReleaseAsset> ReleaseAssets { get; set; } = new List<ReleaseAsset>();

    public virtual Repository Repository { get; set; } = null!;
}
