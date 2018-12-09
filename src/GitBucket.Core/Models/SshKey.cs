namespace GitBucket.Core.Models
{
    public partial class SshKey
    {
        public string UserName { get; set; }
        public int SshKeyId { get; set; }
        public string Title { get; set; }
        public string PublicKey { get; set; }

        public virtual Account UserNameNavigation { get; set; }
    }
}
