using CommandLine;
using System.Collections.Generic;

#nullable disable

namespace GitBucket.Core;

[Verb("milestone", HelpText = "Show milestones")]
public class MilestoneOptions : CommandLineOptionsBase
{
    [Option('o', "owner", Required = true, Separator = ':', HelpText = @"The owner names of the repositories to show milestones. Use "":"" for separator.")]
    public IEnumerable<string> Owners { get; set; } = [];

    [Option('r', "repository", Required = false, Separator = ':', HelpText = @"The repository names to show milestones. Use "":"" for separator.")]
    public IEnumerable<string> Repositories { get; set; } = [];

    [Option('c', "includeClosed", Required = false, HelpText = "Whether show closed milestones.")]
    public bool IncludeClosed { get; set; } = false;
}
