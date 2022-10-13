using SkillBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillBase.ViewModels.Schedule.Year
{
    internal class DayOfYearViewModel : IDisposable
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
            TaskCount = tasks.Count();
            CompletedTaskCount = tasks.Where(x => x.IsCompleted).ToList().Count;
            IsToday = DateTime.Today == Date.Date;
        }

        public void Dispose()
        {

        }

        public DateTime Date { get; set; }
        public bool IsActive { get; set; }
        public int TaskCount { get; set; }
        public int CompletedTaskCount { get; set; }
        public bool IsToday { get; private set; }
    }
}
