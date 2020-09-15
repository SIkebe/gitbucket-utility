using System;
using System.Collections.Generic;

#nullable disable

namespace GitBucket.Core.Models
{
    public partial class NotificationsAccount
    {
        public string UserName { get; set; }
        public bool DisableEmail { get; set; }

        public virtual Account UserNameNavigation { get; set; }
    }
}
