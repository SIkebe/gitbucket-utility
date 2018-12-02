using System;
using System.Linq;
using System.Threading.Tasks;
using GitBucket.Core;
using GitBucket.Core.Models;
using GitBucket.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GitBucket.Service
{
    public interface IMilestoneService
    {
        Task<int> ShowMilestones(MilestoneOptions options);
    }

    public class MilestoneService : IMilestoneService
    {
        private readonly MilestoneRepositoryBase _milestoneRepository;
        private readonly IConsole _console;

        public MilestoneService(MilestoneRepositoryBase milestoneRepository, IConsole console)
            => (_milestoneRepository, _console) = (milestoneRepository, console);

        public async Task<int> ShowMilestones(MilestoneOptions options)
        {
            var milestones = await _milestoneRepository.FindMilestones(options);
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
    }
}
