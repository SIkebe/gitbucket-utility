using GitBucket.Core;
using GitBucket.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GitBucket.Service;

public interface IMilestoneService
{
    Task<int> ShowMilestones(MilestoneOptions options);
}

public class MilestoneService(DbContext context, IConsole console) : IMilestoneService
{
    private readonly DbContext _context = context ?? throw new ArgumentNullException(nameof(context));
    private readonly IConsole _console = console ?? throw new ArgumentNullException(nameof(console));

    public async Task<int> ShowMilestones(MilestoneOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var milestones = await FindMilestones(options);
        if (milestones.Count == 0)
        {
            _console.WriteLine("There are no milestone.");
            return 0;
        }

        var milestoneOrMilestones = milestones.Count == 1 ? "milestone." : "milestones.";
        _console.WriteLine(options.IncludeClosed ?
            $"There are {milestones.Count} {milestoneOrMilestones}" :
            $"There are {milestones.Count} open {milestoneOrMilestones}");
        _console.WriteLine(string.Empty);

        foreach (var milestone in milestones)
        {
            if (milestone.ClosedDate != null)
            {
                _console.WriteLine(milestone.Format());
            }
            else if (milestone.DueDate == null ||
                    (milestone.DueDate >= options.ExecutedDate) &&
                    (milestone.DueDate.Value.Date < options.ExecutedDate.Date.AddDays(7)))
            {
                _console.WriteWarnLine(milestone.Format());
            }
            else if (milestone.DueDate < options.ExecutedDate)
            {
                _console.WriteErrorLine(milestone.Format());
            }
            else
            {
                _console.WriteLine(milestone.Format());
            }
        }

        return 0;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1862:Use the 'StringComparison' method overloads to perform case-insensitive string comparisons", Justification = "Can't be translated")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1304:Specify CultureInfo", Justification = "Can't be translated")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1311:Specify a culture or use an invariant version", Justification = "Can't be translated")]
    private async Task<List<Milestone>> FindMilestones(MilestoneOptions options)
    {
        var owners = options.Owners.Select(o => o.ToLower());
        var repositories = options.Repositories.Select(r => r.ToLower());

        return await _context.Set<Milestone>()
            .WhereIf(options.Owners.Any(), m => owners.Contains(m.UserName.ToLower()))
            .WhereIf(options.Repositories.Any(), m => repositories.Contains(m.RepositoryName.ToLower()))
            .WhereIf(!options.IncludeClosed, m => m.ClosedDate == null)
            .OrderBy(m => m.DueDate)
            .ThenBy(m => m.UserName)
            .ThenBy(m => m.RepositoryName)
            .Include(m => m.Issues)
            .ThenInclude(i => i.IssueAssignees)
            .AsNoTracking()
            .ToListAsync();
    }
}
