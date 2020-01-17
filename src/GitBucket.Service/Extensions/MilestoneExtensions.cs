using System;
using System.Globalization;
using System.Linq;

namespace GitBucket.Core.Models
{
    public static class MilestoneExtensions
    {
        public static string Format(this Milestone milestone)
        {
            if (milestone == null)
            {
                throw new ArgumentNullException(nameof(milestone));
            }

            var assignees = milestone.Issue.Any()
                ? milestone.Issue?.Select(i => i.AssignedUserName).Distinct().OrderBy(a => a).Aggregate((current, next) => $"{current}, {next}")
                : string.Empty;

            var description = milestone.Description?.Replace(Environment.NewLine, " ", ignoreCase: true, CultureInfo.InvariantCulture);
            description = description?.Length > 30 ? description.Substring(0, 30) + "..." : description;
            return @$"* [{milestone.UserName}/{milestone.RepositoryName}], [{milestone.Title}], [{milestone.DueDate?.ToString("yyyy/MM/dd")}], [{description}], [{assignees}]";
        }
    }
}