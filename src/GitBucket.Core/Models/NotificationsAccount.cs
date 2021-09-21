namespace GitBucket.Core.Models;

public partial class NotificationsAccount
{
    public string UserName { get; set; } = null!;
    public bool DisableEmail { get; set; }

    public virtual Account UserNameNavigation { get; set; } = null!;
}
