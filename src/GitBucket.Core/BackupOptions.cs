using CommandLine;
using System.IO;

namespace GitBucket.Core
{
    [Verb("backup", HelpText = "Backup GitBucket repositories and metadata.")]
    public class BackupOptions : CommandLineOptionsBase
    {
        [Option("home", Required = true, HelpText = "GITBUCKET_HOME")]
        public string GitBucketHome { get; set; }

        [Option('d', "dest", Required = true, HelpText = "The backup destination folder path.")]
        public string Destination { get; set; }

        [Option("pgdump", Required = false, HelpText = "The path of pg_dump executable. If pd_dump is not on your PATH, you need to specify this.")]
        public string PgDump { get; set; }

        /// <summary>
        /// Gets the GitBucket configuration file path.
        /// </summary>
        public string GitBucketHomeConfigurationFile => Path.Combine(GitBucketHome, "gitbucket.conf");

        /// <summary>
        /// Gets the GitBucket database configuration file path.
        /// </summary>
        public string GitBucketHomeDatabaseConfigurationFile => Path.Combine(GitBucketHome, "database.conf");

        /// <summary>
        /// Gets the GitBucket repository root folder path.
        /// </summary>
        public string GitBucketHomeRepositoriesDir => Path.Combine(GitBucketHome, "repositories");

        /// <summary>
        /// Gets the GitBucket data folder path.
        /// </summary>
        public string GitBucketHomeDataDir => Path.Combine(GitBucketHome, "data");

        /// <summary>
        /// Gets the GitBucket plugins folder path.
        /// </summary>
        public string GitBucketPluginsDataDir => Path.Combine(GitBucketHome, "plugins");

        /// <summary>
        /// Gets the backup repository root folder path.
        /// </summary>
        public string DestinationRepositoriesDir => Path.Combine(Destination, "repositories");

        /// <summary>
        /// Gets the backup data folder path.
        /// </summary>
        public string DestinationDataDir => Path.Combine(Destination, "data");

        /// <summary>
        /// Gets the backup configuration file path.
        /// </summary>
        public string DestinationConfigurationFile => Path.Combine(Destination, "gitbucket.conf");

        /// <summary>
        /// Gets the backup database configuration file path.
        /// </summary>
        public string DestinationDatabaseConfigurationFile => Path.Combine(Destination, "database.conf");

        /// <summary>
        /// Gets the backup plugins folder path.
        /// </summary>
        public string DestinationPluginsDir => Path.Combine(Destination, "plugins");
    }
}