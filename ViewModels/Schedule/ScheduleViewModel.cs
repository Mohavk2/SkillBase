using Microsoft.Extensions.DependencyInjection;
using SkillBase.ViewModels.Common;
using SkillBase.Views.Schedule.Day;
using SkillBase.Views.Schedule.Week;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillBase.ViewModels.Schedule
{
    internal class ScheduleViewModel : BaseViewModel
    {
        IServiceProvider _serviceProvider;
        public ScheduleViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            DayUC.DataContext = _serviceProvider.GetRequiredService<DayViewModel>();
            WeekUC.DataContext = _serviceProvider.GetRequiredService<WeekViewModel>();
        }
        public DayUC DayUC { get; set; } = new();
        public WeekUC WeekUC { get; set; } = new();
    }
}
