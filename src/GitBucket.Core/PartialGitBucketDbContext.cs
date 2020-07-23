using Microsoft.EntityFrameworkCore;

namespace GitBucket.Core
{
    /// <summary>
    /// DBContext class against GitBucket DB.
    /// Use scaffold.bat to regenerate.
    /// </summary>
    public partial class GitBucketDbContext : DbContext
    {
        public GitBucketDbContext(string connectionString) => ConnectionString = connectionString;

        public string ConnectionString { get; }
    }
}