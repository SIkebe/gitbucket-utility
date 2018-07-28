using CommandLine;

namespace GitBucket.Core
{
    [Verb("release", HelpText = "Output a release note")]
    public class ReleaseOptions
    {
        [Option('o', "owner", Required = true, HelpText = "The owner name of the repository.")]
        public string Owner { get; set; }

        [Option('r', "repository", Required = true, HelpText = "The repository name.")]
        public string Repository { get; set; }

        [Option('m', "milestone", Required = true, HelpText = "The milestone to publish a release note.")]
        public string MileStone { get; set; }

        [Option('t', "target", Required = false, HelpText = "The options to publish a release note based on issues or pull requests.")]
        public string Target { get; set; } = nameof(GitBucket.Core.ReleaseNoteTarget.Issues);
    }
}