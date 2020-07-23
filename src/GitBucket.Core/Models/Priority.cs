using System;
using System.Collections.Generic;

#nullable disable

namespace GitBucket.Core.Models
{
    public partial class Priority
    {
        public Priority()
        {
            Issues = new HashSet<Issue>();
        }

        public string UserName { get; set; }
        public string RepositoryName { get; set; }
        public int PriorityId { get; set; }
        public string PriorityName { get; set; }
        public string Description { get; set; }
        public int Ordering { get; set; }
        public bool IsDefault { get; set; }
        public string Color { get; set; }

        public virtual Repository Repository { get; set; }
        public virtual ICollection<Issue> Issues { get; set; }
    }
}
