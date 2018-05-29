using System;
using System.Collections.Generic;

namespace gbutil.Models
{
    public partial class SshKey
    {
        public string UserName { get; set; }
        public int SshKeyId { get; set; }
        public string Title { get; set; }
        public string PublicKey { get; set; }

        public Account UserNameNavigation { get; set; }
    }
}
