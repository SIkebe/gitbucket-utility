namespace GitBucket.Core.Models;

public partial class GistComment
{
    public string UserName { get; set; } = null!;
    public string RepositoryName { get; set; } = null!;
    public int CommentId { get; set; }
    public string CommentedUserName { get; set; } = null!;
    public string Content { get; set; } = null!;
    public DateTime RegisteredDate { get; set; }
    public DateTime UpdatedDate { get; set; }

    public virtual Account CommentedUserNameNavigation { get; set; } = null!;
    public virtual Gist Gist { get; set; } = null!;
}
