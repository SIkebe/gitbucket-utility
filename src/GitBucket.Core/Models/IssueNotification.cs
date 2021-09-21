namespace GitBucket.Core.Models;

public partial class IssueNotification
{
    public string UserName { get; set; } = null!;
    public string RepositoryName { get; set; } = null!;
    public int IssueId { get; set; }
    public string NotificationUserName { get; set; } = null!;
    public bool Subscribed { get; set; }
}
