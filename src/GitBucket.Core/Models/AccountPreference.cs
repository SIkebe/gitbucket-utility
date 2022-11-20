using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models;

public partial class AccountPreference
{
    public string UserName { get; set; } = null!;

    public string HighlighterTheme { get; set; } = null!;

    public virtual Account UserNameNavigation { get; set; } = null!;
}
