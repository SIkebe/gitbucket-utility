using System;
using System.Collections.Generic;

#nullable disable

namespace GitBucket.Core.Models
{
    public partial class AccountWebHook
    {
        public string UserName { get; set; }
        public string Url { get; set; }
        public string Token { get; set; }
        public string Ctype { get; set; }

        public virtual Account UserNameNavigation { get; set; }
    }
}
