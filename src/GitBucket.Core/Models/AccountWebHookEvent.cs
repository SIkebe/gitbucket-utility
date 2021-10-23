using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models
{
    public partial class AccountWebHookEvent
    {
        public string UserName { get; set; } = null!;
        public string Url { get; set; } = null!;
        public string Event { get; set; } = null!;
    }
}
