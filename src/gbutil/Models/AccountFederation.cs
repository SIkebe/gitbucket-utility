using System;
using System.Collections.Generic;

namespace gbutil.Models
{
    public partial class AccountFederation
    {
        public string Issuer { get; set; }
        public string Subject { get; set; }
        public string UserName { get; set; }

        public Account UserNameNavigation { get; set; }
    }
}
