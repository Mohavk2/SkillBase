using SkillBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillBase.ViewModels.Schedule.Year
{
    internal class DayOfYearViewModel
    {
        IServiceProvider _serviceProvider;
        public DayOfYearViewModel(
            IServiceProvider serviceProvider, 
            DateTime date, 
            IEnumerable<SkillTask> tasks, 
            bool isActive)
        {
            _serviceProvider = serviceProvider;
            Date = date;

            IsActive = isActive;

            var tasksTicks = tasks.Sum(x => x.EndDate != null && x.StartDate != null ?
            ((DateTime)x.EndDate).Ticks - ((DateTime)x.StartDate).Ticks : 0);

            int recommendedBusyHours = 4; //TODO: add to settings
            BusyHoursPercentage = 100 / (recommendedBusyHours / new TimeSpan(tasksTicks).TotalHours);

            IsToday = DateTime.Today == Date.Date;
        }

        public DateTime Date { get; set; }
        public bool IsActive { get; set; }
        public double BusyHoursPercentage { get; set; }
        public bool IsToday { get; private set; }
    }
}
