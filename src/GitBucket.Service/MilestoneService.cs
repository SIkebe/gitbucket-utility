using System;
using System.Linq;
using GitBucket.Core;
using GitBucket.Core.Models;
using GitBucket.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GitBucket.Service
{
    public interface IMilestoneService
    {
        int ShowMilestones(MilestoneOptions options);
    }

    public class MilestoneService : IMilestoneService
    {
        private readonly MilestoneRepositoryBase _milestoneRepository;
        private readonly IConsole _console;

        public MilestoneService(MilestoneRepositoryBase milestoneRepository, IConsole console)
            => (_milestoneRepository, _console) = (milestoneRepository, console);

        public int ShowMilestones(MilestoneOptions options)
        {
            var milestones = _milestoneRepository.FindMilestones(options).ToList();
            if (milestones.Count == 0)
            {
                _console.WriteLine("There are no milestone.");
                return 0;
            }

            var milestoneOrMilestones = milestones.Count == 1 ? "milestone." : "milestones.";
            _console.WriteLine($"There are {milestones.Count} open {milestoneOrMilestones}");
            _console.WriteLine(string.Empty);

            foreach (var milestone in milestones)
            {
                if (milestone.DueDate != null && milestone.DueDate < options.ExecutedDate)
                {
                    _console.WriteWarn("* ");
                    _console.WriteWarn(milestone.RepositoryName);
                    _console.WriteWarn(", ");
                    _console.WriteWarn(milestone.Title);
                    _console.WriteWarn(", ");
                    _console.WriteWarn(milestone.DueDate?.ToLocalTime().ToShortDateString());
                    _console.WriteWarn(", ");
                    _console.WriteWarnLine(milestone.Description);
                    continue;
                }

                _console.Write("* ");
                _console.Write(milestone.RepositoryName);
                _console.Write(", ");
                _console.Write(milestone.Title);
                _console.Write(", ");
                _console.Write(milestone.DueDate?.ToLocalTime().ToShortDateString());
                _console.Write(", ");
                _console.WriteLine(milestone.Description);
            }

            return 0;
        }
    }
}
