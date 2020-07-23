using CommandLine;

#nullable disable

namespace GitBucket.Core
{
    [Verb("release", HelpText = "Output a release note")]
    public class ReleaseOptions : CommandLineOptionsBase
    {
        [Option('o', "owner", Required = true, HelpText = "The owner name of the repository.")]
        public string Owner { get; set; }

        [Option('r', "repository", Required = true, HelpText = "The repository name.")]
        public string Repository { get; set; }

        [Option('m', "milestone", Required = true, HelpText = "The milestone to publish a release note.")]
        public string MileStone { get; set; }

        [Option("from-pr", Required = false, HelpText = "If specified, gbutil publish a release note based on pull requests.")]
        public bool FromPullRequest { get; set; }

        [Option("create-pr", Required = false, Default = false, HelpText = "Whether create pull request based on the milestone.")]
        public bool CreatePullRequest { get; set; }

        [Option("base", Required = false, Default = "master", HelpText = "The name of the branch you want the changes pulled into.")]
        public string Base { get; set; } = "master";

        [Option('h', "head", Required = false, Default = "develop", HelpText = "The name of the branch where your changes are implemented.")]
        public string Head { get; set; } = "develop";

        [Option("title", Required = false, HelpText = "The title of the new pull request. Default value is the same as milestone.")]
        public string Title { get; set; }

        [Option('f', "force", Required = false, HelpText = "Force the output/creating PR if specified.")]
        public bool Force { get; set; }

        [Option("draft", Required = false, Default = false, HelpText = "If specified, create draft pull request.")]
        public bool Draft { get; set; }
    }
}