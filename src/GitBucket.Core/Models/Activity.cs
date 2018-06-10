using System;
using System.Collections.Generic;

namespace GitBucket.Core.Models
{
    public partial class Activity
    {
        public int ActivityId { get; set; }
        public string UserName { get; set; }
        public string RepositoryName { get; set; }
        public string ActivityUserName { get; set; }
        public string ActivityType { get; set; }
        public string Message { get; set; }
        public string AdditionalInfo { get; set; }
        public DateTime ActivityDate { get; set; }

        public Account ActivityUserNameNavigation { get; set; }
        public Repository Repository { get; set; }
    }
}
