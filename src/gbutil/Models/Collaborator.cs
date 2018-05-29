using System;
using System.Collections.Generic;

namespace gbutil.Models
{
    public partial class Collaborator
    {
        public string UserName { get; set; }
        public string RepositoryName { get; set; }
        public string CollaboratorName { get; set; }
        public string Role { get; set; }

        public Account CollaboratorNameNavigation { get; set; }
        public Repository Repository { get; set; }
    }
}
