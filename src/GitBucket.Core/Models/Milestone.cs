using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models
{
    public partial class Milestone
    {
        public Milestone()
        {
            Issue = new HashSet<Issue>();
        }

        public string UserName { get; set; }
        public string RepositoryName { get; set; }
        public int MilestoneId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? ClosedDate { get; set; }

        public Repository Repository { get; set; }
        public ICollection<Issue> Issue { get; set; }
    }
}
