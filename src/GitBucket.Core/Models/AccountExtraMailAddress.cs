using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models;

public partial class AccountExtraMailAddress
{
    public string UserName { get; set; } = null!;
    public string ExtraMailAddress { get; set; } = null!;
}
