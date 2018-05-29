using System;
using System.Collections.Generic;

namespace gbutil.Models
{
    public partial class ReleaseAsset
    {
        public string UserName { get; set; }
        public string RepositoryName { get; set; }
        public string Tag { get; set; }
        public int ReleaseAssetId { get; set; }
        public string FileName { get; set; }
        public string Label { get; set; }
        public long Size { get; set; }
        public string Uploader { get; set; }
        public DateTime RegisteredDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public ReleaseTag ReleaseTag { get; set; }
    }
}
