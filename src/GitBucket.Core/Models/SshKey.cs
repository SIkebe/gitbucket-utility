using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models;

public partial class SshKey
{
    public string UserName { get; set; } = null!;
    public int SshKeyId { get; set; }
    public string Title { get; set; } = null!;
    public string PublicKey { get; set; } = null!;

    public virtual Account UserNameNavigation { get; set; } = null!;
}
