using System;
using System.Collections.Generic;

#nullable disable

namespace GitBucket.Core.Models
{
    public partial class AccountPreference
    {
        public string UserName { get; set; }
        public string HighlighterTheme { get; set; }

        public virtual Account UserNameNavigation { get; set; }
    }
}
