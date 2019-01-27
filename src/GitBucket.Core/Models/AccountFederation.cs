namespace GitBucket.Core.Models
{
    public partial class AccountFederation
    {
        public string Issuer { get; set; }
        public string Subject { get; set; }
        public string UserName { get; set; }

        public virtual Account UserNameNavigation { get; set; }
    }
}
