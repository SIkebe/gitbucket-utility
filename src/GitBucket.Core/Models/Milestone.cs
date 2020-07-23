using System;
using System.Collections.Generic;

#nullable disable

namespace GitBucket.Core.Models
{
    public partial class Milestone
    {
        public Milestone()
        {
            Issues = new HashSet<Issue>();
        }

        public string UserName { get; set; }
        public string RepositoryName { get; set; }
        public int MilestoneId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? ClosedDate { get; set; }

        public virtual Repository Repository { get; set; }
        public virtual ICollection<Issue> Issues { get; set; }
    }
}
