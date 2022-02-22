using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models
{
    public partial class Priority
    {
        public Priority()
        {
            Issues = new HashSet<Issue>();
        }

        public string UserName { get; set; } = null!;
        public string RepositoryName { get; set; } = null!;
        public int PriorityId { get; set; }
        public string PriorityName { get; set; } = null!;
        public string? Description { get; set; }
        public int Ordering { get; set; }
        public bool IsDefault { get; set; }
        public string Color { get; set; } = null!;

        public virtual Repository Repository { get; set; } = null!;
        public virtual ICollection<Issue> Issues { get; set; }
    }
}
