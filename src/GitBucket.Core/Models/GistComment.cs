using System;

namespace GitBucket.Core.Models
{
    public partial class GistComment
    {
        public string UserName { get; set; }
        public string RepositoryName { get; set; }
        public int CommentId { get; set; }
        public string CommentedUserName { get; set; }
        public string Content { get; set; }
        public DateTime RegisteredDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public Account CommentedUserNameNavigation { get; set; }
        public Gist Gist { get; set; }
    }
}
