using System;
using System.Collections.Generic;

#nullable disable

namespace GitBucket.Core.Models
{
    public partial class IssueLabel
    {
        public string UserName { get; set; }
        public string RepositoryName { get; set; }
        public int IssueId { get; set; }
        public int LabelId { get; set; }

        public virtual Issue Issue { get; set; }
    }
}
