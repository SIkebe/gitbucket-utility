using System;

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

            return $"* {milestone.UserName}/{milestone.RepositoryName}, {milestone.Title}, {milestone.DueDate?.ToString("yyyy/MM/dd")}, {milestone.Description}";
        }
    }
}