using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models;

public partial class Page
{
    public string UserName { get; set; } = null!;
    public string RepositoryName { get; set; } = null!;
    public string Source { get; set; } = null!;
}
