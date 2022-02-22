using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models;

public partial class ReleaseAsset
{
    public string UserName { get; set; } = null!;
    public string RepositoryName { get; set; } = null!;
    public string Tag { get; set; } = null!;
    public int ReleaseAssetId { get; set; }
    public string FileName { get; set; } = null!;
    public string? Label { get; set; }
    public long Size { get; set; }
    public string Uploader { get; set; } = null!;
    public DateTime RegisteredDate { get; set; }
    public DateTime UpdatedDate { get; set; }

    public virtual ReleaseTag ReleaseTag { get; set; } = null!;
}
