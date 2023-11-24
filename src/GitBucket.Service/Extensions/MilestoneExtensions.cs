using System.Globalization;

namespace GitBucket.Core.Models;

public static class MilestoneExtensions
{
    public static string Format(this Milestone milestone)
    {
        ArgumentNullException.ThrowIfNull(milestone);

        var assignees = string.Empty;
        if (milestone.Issues.Any())
        {
            var assigneeUserNames = milestone.Issues.SelectMany(i => i.IssueAssignees.Select(i => i.AssigneeUserName));
            if (assigneeUserNames is not null && assigneeUserNames.Any())
            {
                assignees = assigneeUserNames.Distinct().OrderBy(a => a).Aggregate((current, next) => $"{current}, {next}");
            }
        }

        var description = milestone.Description?.Replace(Environment.NewLine, " ", ignoreCase: true, CultureInfo.InvariantCulture);
        description = description?.Length > 30 ? string.Concat(description.AsSpan(0, 30), "...") : description;
        return $@"* [{milestone.UserName}/{milestone.RepositoryName}], [{milestone.Title}], [{milestone.DueDate?.ToLocalTime().ToString("yyyy/MM/dd")}], [{description}], [{assignees}]";
    }
}
