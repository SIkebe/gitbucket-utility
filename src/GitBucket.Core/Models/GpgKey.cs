namespace GitBucket.Core.Models;

public partial class GpgKey
{
    public string UserName { get; set; } = null!;
    public int KeyId { get; set; }
    public long GpgKeyId { get; set; }
    public string Title { get; set; } = null!;
    public string PublicKey { get; set; } = null!;

    public virtual Account UserNameNavigation { get; set; } = null!;
}
