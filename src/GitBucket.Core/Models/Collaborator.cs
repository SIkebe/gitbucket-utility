namespace GitBucket.Core.Models;

public partial class Collaborator
{
    public string UserName { get; set; } = null!;
    public string RepositoryName { get; set; } = null!;
    public string CollaboratorName { get; set; } = null!;
    public string Role { get; set; } = null!;

    public virtual Account CollaboratorNameNavigation { get; set; } = null!;
    public virtual Repository Repository { get; set; } = null!;
}
