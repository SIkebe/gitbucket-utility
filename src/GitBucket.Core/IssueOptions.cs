using CommandLine;
using CommandLine.Text;

namespace GitBucket.Core
{
    [Verb("issue", HelpText = "Move an issue to a different repository")]
    public class IssueOptions : CommandLineOptionsBase
    {
        [Option('t', "type", Required = false, HelpText = @"The type of issue options. Default value is ""move"".")]
        public string Type { get; set; } = "move";

        [Option('s', "source", Required = true, HelpText = @"The source owner and repository to move from. Use ""/"" for separator like ""root/repository1"".")]
        public string Source { get; set; }

        [Option('d', "destination", Required = true, HelpText = @"The destination owner and repository to move to. Use ""/"" for separator like ""root/repository2"".")]
        public string Destination { get; set; }

        [Option('n', "number", Required = true, HelpText = @"The issue number to move.")]
        public int IssueNumber { get; set; }
    }
}