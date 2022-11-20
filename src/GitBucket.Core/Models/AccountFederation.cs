using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models;

public partial class AccountFederation
{
    public string Issuer { get; set; } = null!;

    public string Subject { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public virtual Account UserNameNavigation { get; set; } = null!;
}
