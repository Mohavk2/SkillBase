using SkillBase.Models;
using SkillBase.ViewModels.Common;
using SkillBase.ViewModels.Schedule.Year;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillBase.ViewModels.Factories
{
    internal class DayOfYearViewModelFactory : BaseViewModel
    {
        IServiceProvider _serviceProvider;
        public DayOfYearViewModelFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public DayOfYearViewModel Create(DateTime date, IEnumerable<SkillTask> tasks, bool isActive = true)
        {
            return new DayOfYearViewModel(_serviceProvider, date, tasks, isActive);
        }
    }
}
