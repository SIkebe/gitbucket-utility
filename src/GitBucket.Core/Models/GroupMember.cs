namespace GitBucket.Core.Models;

public partial class GroupMember
{
    public string GroupName { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public bool? Manager { get; set; }

    public virtual Account GroupNameNavigation { get; set; } = null!;
    public virtual Account UserNameNavigation { get; set; } = null!;
}
