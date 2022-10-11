using SkillBase.Models;
using SkillBase.ViewModels.Schedule.Month;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillBase.ViewModels.Factories
{
    internal class DayOfMonthViewModelFactory
    {
        IServiceProvider _serviceProvider;
        public DayOfMonthViewModelFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public DayOfMonthViewModel Create(DateTime date, IEnumerable<SkillTask> tasks, bool isActive = true)
        {
            return new DayOfMonthViewModel(_serviceProvider, date, tasks, isActive);
        }
    }
}
