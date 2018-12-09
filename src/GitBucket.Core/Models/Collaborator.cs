using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models
{
    public partial class Collaborator
    {
        public string UserName { get; set; }
        public string RepositoryName { get; set; }
        public string CollaboratorName { get; set; }
        public string Role { get; set; }

        public virtual Account CollaboratorNameNavigation { get; set; }
        public virtual Repository Repository { get; set; }
    }
}
