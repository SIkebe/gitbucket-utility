using System;
using System.Collections.Generic;

namespace gbutil.Models
{
    public partial class IssueNotification
    {
        public string UserName { get; set; }
        public string RepositoryName { get; set; }
        public int IssueId { get; set; }
        public string NotificationUserName { get; set; }
        public bool Subscribed { get; set; }
    }
}
