using SkillBase.Models;
using SkillBase.ViewModels.Schedule.Year;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillBase.ViewModels.Factories
{
    internal class MonthOfYearViewModelFactory
    {
        IServiceProvider _serviceProvider;
        public MonthOfYearViewModelFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public MonthOfYearViewModel Create(DateTime date, IEnumerable<SkillTask> tasks)
        {
            return new MonthOfYearViewModel(_serviceProvider, date, tasks);
        }
    }
}
