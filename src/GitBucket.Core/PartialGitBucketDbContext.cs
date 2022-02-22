using Microsoft.EntityFrameworkCore;
using System;

namespace GitBucket.Core;

/// <summary>
/// DBContext class against GitBucket DB.
/// Use scaffold.bat to regenerate.
/// </summary>
public partial class GitBucketDbContext : DbContext
{
    public GitBucketDbContext(string connectionString) => ConnectionString = connectionString;

    public string? ConnectionString { get; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder is null)
        {
            throw new System.ArgumentNullException(nameof(optionsBuilder));
        }

        if (!optionsBuilder.IsConfigured)
        {
            if (ConnectionString is null)
            {
                throw new InvalidOperationException(nameof(ConnectionString));
            }

            optionsBuilder.UseNpgsql(ConnectionString);
        }
    }
}
