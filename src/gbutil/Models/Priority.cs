using System;
using System.Collections.Generic;

namespace gbutil.Models
{
    public partial class Priority
    {
        public Priority()
        {
            Issue = new HashSet<Issue>();
        }

        public string UserName { get; set; }
        public string RepositoryName { get; set; }
        public int PriorityId { get; set; }
        public string PriorityName { get; set; }
        public string Description { get; set; }
        public int Ordering { get; set; }
        public bool IsDefault { get; set; }
        public string Color { get; set; }

        public Repository Repository { get; set; }
        public ICollection<Issue> Issue { get; set; }
    }
}
