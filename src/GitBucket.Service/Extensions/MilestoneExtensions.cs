namespace GitBucket.Core.Models
{
    public static class MilestoneExtensions
    {
        public static string Format(this Milestone milestone)
            => $"* {milestone.UserName}/{milestone.RepositoryName}, {milestone.Title}, {milestone.DueDate?.ToString("yyyy/MM/dd")}, {milestone.Description}";
    }
}