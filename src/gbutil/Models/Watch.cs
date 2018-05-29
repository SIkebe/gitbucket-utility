using System;
using System.Collections.Generic;

namespace gbutil.Models
{
    public partial class Watch
    {
        public string UserName { get; set; }
        public string RepositoryName { get; set; }
        public string NotificationUserName { get; set; }
        public string Notification { get; set; }
    }
}
