using System;
using System.Linq;
using GitBucket.Core;
using GitBucket.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GitBucket.Service
{
    public interface IReleaseNoteService
    {
        int OutputReleaseNotes(ReleaseOptions options);
    }

    public class ReleaseNoteService : IReleaseNoteService
    {
        private readonly IssueRepositoryBase _issueRepository;
        private readonly LabelRepositoryBase _labelRepository;
        private IConsole _console;

        public ReleaseNoteService(
            IssueRepositoryBase issueRepository,
            LabelRepositoryBase labelRepository,
            IConsole console)
        {
            _issueRepository = issueRepository;
            _labelRepository = labelRepository;
            _console = console;
        }

        public int OutputReleaseNotes(ReleaseOptions options)
        {
            var closedTargets = options.Target.ToLowerInvariant();
            var issues = _issueRepository.FindIssuesRelatedToMileStone(options).ToList();
            if (!issues.Any())
            {
                _console.WriteWarnLine($"There are no {closedTargets} related to \"{options.MileStone}\".");
                return 1;
            }

            if (issues.Any(i => !i.Closed))
            {
                _console.WriteWarnLine($"There are unclosed {closedTargets} in \"{options.MileStone}\".");
                _console.WriteWarn("Do you want to continue?([Y]es/[N]o): ");
                string yesOrNo = Console.ReadLine();

                if (!string.Equals(yesOrNo, "y", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(yesOrNo, "yes", StringComparison.OrdinalIgnoreCase))
                {
                    return 1;
                }

                _console.WriteLine("");
            }

            var issueLabels = _issueRepository.FindIssueLabels(options, issues).ToList();
            if (issues.Any(i => !issueLabels.Select(l => l.IssueId).Contains(i.IssueId)))
            {
                _console.WriteWarnLine($"There are issues which have no labels in \"{options.MileStone}\".");
                return 1;
            }

            var labels = _labelRepository.FindBy(l =>
                l.UserName == options.Owner &&
                l.RepositoryName == options.Repository &&
                issueLabels.Select(i => i.LabelId).Contains(l.LabelId));

            var highestPriority = issues
                .OrderBy(i => i.Priority.Ordering)
                .First()
                .Priority.PriorityName;

            _console.WriteLine($"As part of this release we had {issues.Count} {closedTargets} closed.");
            _console.WriteLine($"The highest priority among them is \"{highestPriority}\".");
            _console.WriteLine("");
            foreach (var label in labels)
            {
                _console.WriteLine($"### {label.LabelName.ConvertFirstCharToUpper()}");

                var ids = issueLabels
                    .Where(l => l.LabelId == label.LabelId)
                    .Select(i => i.IssueId)
                    .OrderBy(i => i);

                foreach (var issueId in ids)
                {
                    var issue = issues.Where(i => i.IssueId == issueId).Single();
                    _console.WriteLine($"* {issue.Title} #{issue.IssueId}");
                }

                _console.WriteLine("");
            }

            return 0;
        }
    }
}
