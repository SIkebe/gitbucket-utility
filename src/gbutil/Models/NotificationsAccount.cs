using System;
using System.Collections.Generic;

namespace gbutil.Models
{
    public partial class NotificationsAccount
    {
        public string UserName { get; set; }
        public bool DisableEmail { get; set; }

        public Account UserNameNavigation { get; set; }
    }
}
