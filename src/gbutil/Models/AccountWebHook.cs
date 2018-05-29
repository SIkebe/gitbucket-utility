using System;
using System.Collections.Generic;

namespace gbutil.Models
{
    public partial class AccountWebHook
    {
        public string UserName { get; set; }
        public string Url { get; set; }
        public string Token { get; set; }
        public string Ctype { get; set; }

        public Account UserNameNavigation { get; set; }
    }
}
