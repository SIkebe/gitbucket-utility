using System;
using System.Collections.Generic;

#nullable disable

namespace GitBucket.Core.Models
{
    public partial class Gist
    {
        public Gist()
        {
            GistComments = new HashSet<GistComment>();
        }

        public string UserName { get; set; }
        public string RepositoryName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string OriginUserName { get; set; }
        public string OriginRepositoryName { get; set; }
        public DateTime RegisteredDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Mode { get; set; }

        public virtual Account UserNameNavigation { get; set; }
        public virtual ICollection<GistComment> GistComments { get; set; }
    }
}
