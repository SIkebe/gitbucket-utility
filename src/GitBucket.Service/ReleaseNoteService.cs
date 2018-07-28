using System;
using System.Linq;
using GitBucket.Core;
using GitBucket.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GitBucket.Service
{
    public interface IReleaseNoteService
    {
        int OutputReleaseNotes(ReleaseOptions option);
    }

    public class ReleaseNoteService : IReleaseNoteService
    {
        private readonly IssueRepositoryBase _issueRepository;
        private readonly LabelRepositoryBase _labelRepository;

        public ReleaseNoteService(
            IssueRepositoryBase issueRepository,
            LabelRepositoryBase labelRepository)
        {
            _issueRepository = issueRepository;
            _labelRepository = labelRepository;
        }

        public int OutputReleaseNotes(ReleaseOptions option)
        {
            var closedTargets = option.Target.ToLowerInvariant();
            var issues = _issueRepository.FindIssuesRelatedToMileStone(option).ToList();
            if (!issues.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"There are no {closedTargets} related to \"{option.MileStone}\".");
                Console.ResetColor();
                return 1;
            }

            if (issues.Any(i => !i.Closed))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"There are unclosed {closedTargets} in \"{option.MileStone}\".");
                Console.Write("Do you want to continue?([Y]es/[N]o): ");
                Console.ResetColor();

                string yesOrNo = Console.ReadLine();

                if (!string.Equals(yesOrNo, "y", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(yesOrNo, "yes", StringComparison.OrdinalIgnoreCase))
                {
                    return 1;
                }

                Console.WriteLine("");
            }

            var issueLabels = _issueRepository.FindIssueLabels(option, issues).ToList();
            if (issues.Any(i => !issueLabels.Select(l => l.IssueId).Contains(i.IssueId)))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"There are issues which have no labels in \"{option.MileStone}\".");
                Console.ResetColor();
                return 1;
            }

            var labels = _labelRepository.FindBy(l =>
                l.UserName == option.Owner &&
                l.RepositoryName == option.Repository &&
                issueLabels.Select(i => i.LabelId).Contains(l.LabelId));

            var highestPriority = issues
                .OrderBy(i => i.Priority.Ordering)
                .First()
                .Priority.PriorityName;

            Console.WriteLine($"As part of this release we had {issues.Count} {closedTargets} closed.");
            Console.WriteLine($"The highest priority among them is \"{highestPriority}\".");
            Console.WriteLine("");
            foreach (var label in labels)
            {
                Console.WriteLine($"### {label.LabelName.ConvertFirstCharToUpper()}");

                var ids = issueLabels
                    .Where(l => l.LabelId == label.LabelId)
                    .Select(i => i.IssueId)
                    .OrderBy(i => i);

                foreach (var issueId in ids)
                {
                    var issue = issues.Where(i => i.IssueId == issueId).Single();
                    Console.WriteLine($"* {issue.Title} #{issue.IssueId}");
                }

                Console.WriteLine("");
            }

            return 0;
        }
    }
}
