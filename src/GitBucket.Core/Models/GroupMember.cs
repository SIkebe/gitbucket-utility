using System;
using System.Collections.Generic;

#nullable disable

namespace GitBucket.Core.Models
{
    public partial class GroupMember
    {
        public string GroupName { get; set; }
        public string UserName { get; set; }
        public bool? Manager { get; set; }

        public virtual Account GroupNameNavigation { get; set; }
        public virtual Account UserNameNavigation { get; set; }
    }
}
