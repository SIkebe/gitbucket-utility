using System;
using System.Collections.Generic;

#nullable disable

namespace GitBucket.Core.Models
{
    public partial class AccountWebHookEvent
    {
        public string UserName { get; set; }
        public string Url { get; set; }
        public string Event { get; set; }
    }
}
