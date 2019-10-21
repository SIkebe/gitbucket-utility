using System.Collections.Generic;
using CommandLine;

namespace GitBucket.Core
{
    [Verb("compare", HelpText = "Compare branches")]
    public class CompareOptions : CommandLineOptionsBase
    {
        [Option('o', "owner", Required = true, Separator = ':', HelpText = @"The owner names of the repositories to compare branches. Use "":"" for separator.")]
        public IEnumerable<string> Owners { get; set; } = new List<string>();

        [Option("compare1", Required = false, Default = "master", HelpText = "The name of the branch you want to compare the changes.")]
        public string Compare1 { get; set; } = "master";

        [Option("compare2", Required = false, Default = "develop", HelpText = "The name of the another branch where you want to compare the changes.")]
        public string Compare2 { get; set; } = "develop";
    }
}