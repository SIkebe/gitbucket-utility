using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models;

public partial class WebHookEvent
{
    public string UserName { get; set; } = null!;
    public string RepositoryName { get; set; } = null!;
    public string Url { get; set; } = null!;
    public string Event { get; set; } = null!;

    public virtual WebHook WebHook { get; set; } = null!;
}
