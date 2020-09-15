using System;
using System.Collections.Generic;

#nullable disable

namespace GitBucket.Core.Models
{
    public partial class Label
    {
        public string UserName { get; set; }
        public string RepositoryName { get; set; }
        public int LabelId { get; set; }
        public string LabelName { get; set; }
        public string Color { get; set; }

        public virtual Repository Repository { get; set; }
    }
}
