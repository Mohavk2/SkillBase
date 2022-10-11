using Microsoft.Extensions.DependencyInjection;
using SkillBase.ViewModels.Common;
using SkillBase.ViewModels.Schedule.Day;
using SkillBase.ViewModels.Schedule.Month;
using SkillBase.ViewModels.Schedule.Week;
using SkillBase.ViewModels.Schedule.Year;
using SkillBase.Views.Schedule.Day;
using SkillBase.Views.Schedule.Month;
using SkillBase.Views.Schedule.Week;
using SkillBase.Views.Schedule.Year;
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
            MonthUC.DataContext = _serviceProvider.GetRequiredService<MonthViewModel>();
            YearUC.DataContext = _serviceProvider.GetRequiredService<YearViewModel>();
        }
        public DayUC DayUC { get; set; } = new();
        public WeekUC WeekUC { get; set; } = new();
        public MonthUC MonthUC { get; set; } = new();
        public YearUC YearUC { get; set; } = new();
    }
}
