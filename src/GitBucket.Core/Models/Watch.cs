namespace GitBucket.Core.Models;

public partial class Watch
{
    public string UserName { get; set; } = null!;
    public string RepositoryName { get; set; } = null!;
    public string NotificationUserName { get; set; } = null!;
    public string Notification { get; set; } = null!;
}
