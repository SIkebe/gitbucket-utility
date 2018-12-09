using System.Collections.Generic;
using CommandLine;

namespace GitBucket.Core
{
    [Verb("issue", HelpText = "Move or Copy an issue to a different repository")]
    public class IssueOptions : CommandLineOptionsBase
    {
        [Option('t', "type", Required = false, HelpText = @"The type of issue options. Default value is ""move"".")]
        public string Type { get; set; } = "move";

        [Option('s', "source", Separator = '/', Min = 2, Max = 2, HelpText = @"The source owner and repository to move/copy from. Use ""/"" for separator like ""root/repository1"".")]
        public IEnumerable<string> Source { get; set; }

        [Option('d', "destination", Separator = '/', Min = 2, Max = 2, HelpText = @"The destination owner and repository to move/copy to. Use ""/"" for separator like ""root/repository2"".")]
        public IEnumerable<string> Destination { get; set; }

        [Option('n', "number", Required = true, HelpText = @"The issue numbers to move/copy. Use "":"" for separator.")]
        public IEnumerable<int> IssueNumbers { get; set; }
    }
}