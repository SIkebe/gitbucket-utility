using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models;

public partial class Gist
{
    public string UserName { get; set; } = null!;

    public string RepositoryName { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? OriginUserName { get; set; }

    public string? OriginRepositoryName { get; set; }

    public DateTime RegisteredDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public string Mode { get; set; } = null!;

    public virtual ICollection<GistComment> GistComments { get; set; } = new List<GistComment>();

    public virtual Account UserNameNavigation { get; set; } = null!;
}
