using CommandLine;

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

        [Option('t', "target", Required = false, HelpText = "The options to publish a release note based on issues or pull requests.")]
        public string Target { get; set; } = nameof(GitBucket.Core.ReleaseNoteTarget.Issues);

        [Option("create-pullrequest", Required = false, Default = false, HelpText = "Whether create pull request based on the milestone.")]
        public bool CreatePullRequest { get; set; }

        [Option('h', "head", Required = false, Default = "develop", HelpText = "The name of the branch where your changes are implemented.")]
        public string Head { get; set; }

        [Option("base", Required = false, Default = "master", HelpText = "The name of the branch you want the changes pulled into.")]
        public string Base { get; set; }

        [Option("body", Required = false, HelpText = "The contents of the pull request.")]
        public string Body { get; set; }

        [Option("title", Required = false, HelpText = "The title of the pull request. Default value is the same as milestone.")]
        public string Title { get; set; }
    }
}