using System.Collections.Generic;
using CommandLine;

namespace GitBucket.Core
{
    [Verb("milestone", HelpText = "Show milestones")]
    public class MilestoneOptions : CommandLineOptionsBase
    {
        [Option('o', "owner", Required = true, Separator = ':', HelpText = @"The owner names of the repositories to show milestones. Use "":"" for separator.")]
        public IEnumerable<string> Owners { get; set; } = new List<string>();

        [Option('r', "repository", Required = false, HelpText = @"The repository names to show milestones. Use "":"" for separator.")]
        public IEnumerable<string> Repositories { get; set; } = new List<string>();

        [Option('c', "includeClosed", Required = false, HelpText = "Whether show closed milestones.")]
        public bool IncludeClosed { get; set; } = false;
    }
}