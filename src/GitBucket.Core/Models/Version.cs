using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models;

public partial class Version
{
    public string ModuleId { get; set; } = null!;
    public string Version1 { get; set; } = null!;
}
