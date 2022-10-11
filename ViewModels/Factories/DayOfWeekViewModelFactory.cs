using SkillBase.Models;
using SkillBase.ViewModels.Schedule.Week;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillBase.ViewModels.Factories
{
    internal class DayOfWeekViewModelFactory
    {
        IServiceProvider _serviceProvider;
        public DayOfWeekViewModelFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public DayOfWeekViewModel Create(DateTime date, IEnumerable<SkillTask> tasks)
        {
            return new(_serviceProvider, date, tasks);
        }
    }
}
