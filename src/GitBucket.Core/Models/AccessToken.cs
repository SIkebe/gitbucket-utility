using System;
using System.Collections.Generic;

#nullable disable

namespace GitBucket.Core.Models
{
    public partial class AccessToken
    {
        public int AccessTokenId { get; set; }
        public string TokenHash { get; set; }
        public string UserName { get; set; }
        public string Note { get; set; }

        public virtual Account UserNameNavigation { get; set; }
    }
}
