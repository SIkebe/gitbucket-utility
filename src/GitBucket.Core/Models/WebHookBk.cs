namespace GitBucket.Core.Models;

public partial class WebHookBk
{
    public string UserName { get; set; } = null!;
    public string RepositoryName { get; set; } = null!;
    public string Url { get; set; } = null!;
    public string? Token { get; set; }
    public string? Ctype { get; set; }
}
