using System;
using System.Collections.Generic;

#nullable disable

namespace GitBucket.Core.Models
{
    public partial class GpgKey
    {
        public string UserName { get; set; }
        public int KeyId { get; set; }
        public long GpgKeyId { get; set; }
        public string Title { get; set; }
        public string PublicKey { get; set; }

        public virtual Account UserNameNavigation { get; set; }
    }
}
