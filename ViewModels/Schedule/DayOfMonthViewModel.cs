using SkillBase.Models;
using SkillBase.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillBase.ViewModels.Schedule
{
    internal class DayOfMonthViewModel : BaseViewModel
    {
        IServiceProvider _serviceProvider;
        public DayOfMonthViewModel(
            IServiceProvider serviceProvider,
            DateTime date, IEnumerable<SkillTask> tasks,
            bool isActive = true)
        {
            _serviceProvider = serviceProvider;
            Date = date;
            IsActive = isActive;
            TaskCount = tasks.Count();

            var tasksTicks = tasks.Sum(x => (x.EndDate != null && x.StartDate != null) ?
            ((DateTime)x.EndDate).Ticks - ((DateTime)x.StartDate).Ticks : 0);

            int recommendedBusyHours = 4; //TODO: add to settings
            BusyHoursPercentage = 100 / (recommendedBusyHours / new TimeSpan(tasksTicks).TotalHours);

            IsToday = DateTime.Today == Date.Date;
        }

        public DateTime Date { get; private set; }
        public bool IsActive { get; private set; }
        public int TaskCount { get; private set; }
        public double BusyHoursPercentage { get; set; }
        public bool IsToday { get; private set; }
    }
}
