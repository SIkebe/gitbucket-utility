using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models
{
    public partial class AccountWebHook
    {
        public string UserName { get; set; } = null!;
        public string Url { get; set; } = null!;
        public string? Token { get; set; }
        public string? Ctype { get; set; }

        public virtual Account UserNameNavigation { get; set; } = null!;
    }
}
