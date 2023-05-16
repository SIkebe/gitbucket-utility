using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models;

public partial class WebHook
{
    public int HookId { get; set; }

    public string UserName { get; set; } = null!;

    public string RepositoryName { get; set; } = null!;

    public string Url { get; set; } = null!;

    public string? Token { get; set; }

    public string? Ctype { get; set; }

    public virtual Repository Repository { get; set; } = null!;

    public virtual ICollection<WebHookEvent> WebHookEvents { get; set; } = new List<WebHookEvent>();
}
