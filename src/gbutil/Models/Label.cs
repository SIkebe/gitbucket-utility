﻿using System;
using System.Collections.Generic;

namespace gbutil.Models
{
    public partial class Label
    {
        public string UserName { get; set; }
        public string RepositoryName { get; set; }
        public int LabelId { get; set; }
        public string LabelName { get; set; }
        public string Color { get; set; }

        public Repository Repository { get; set; }
    }
}
