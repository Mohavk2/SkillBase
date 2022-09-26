using Microsoft.Extensions.DependencyInjection;
using SkillBase.ViewModels.Common;
using SkillBase.Views.Schedule.Day;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillBase.ViewModels
{
    internal class ScheduleViewModel : BaseViewModel
    {
        IServiceProvider _serviceProvider;
        public ScheduleViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            DayUC.DataContext = _serviceProvider.GetRequiredService<DayViewModel>();
        }
        public DayUC DayUC { get; set; } = new();
    }
}
