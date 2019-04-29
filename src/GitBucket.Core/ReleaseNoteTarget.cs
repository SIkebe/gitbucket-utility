namespace GitBucket.Core
{
    /// <summary>
    /// Determine how to create release notes based on issue type.
    /// </summary>
    public enum ReleaseNoteTarget
    {
        /// <summary>
        /// Create release notes based on issues.
        /// </summary>
        Issues = 0,

        /// <summary>
        /// Create release notes based on pull requests.
        /// </summary>
        PullRequests,
    }
}