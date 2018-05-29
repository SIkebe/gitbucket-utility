using System;
using System.Collections.Generic;

namespace gbutil.Models
{
    public partial class GroupMember
    {
        public string GroupName { get; set; }
        public string UserName { get; set; }
        public bool? Manager { get; set; }

        public Account GroupNameNavigation { get; set; }
        public Account UserNameNavigation { get; set; }
    }
}
