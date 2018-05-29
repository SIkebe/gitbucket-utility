using System;
using System.Collections.Generic;

namespace gbutil.Models
{
    public partial class AccessToken
    {
        public int AccessTokenId { get; set; }
        public string TokenHash { get; set; }
        public string UserName { get; set; }
        public string Note { get; set; }

        public Account UserNameNavigation { get; set; }
    }
}
