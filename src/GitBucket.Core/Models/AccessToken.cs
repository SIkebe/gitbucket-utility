using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models;

public partial class AccessToken
{
    public int AccessTokenId { get; set; }
    public string TokenHash { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Note { get; set; } = null!;

    public virtual Account UserNameNavigation { get; set; } = null!;
}
