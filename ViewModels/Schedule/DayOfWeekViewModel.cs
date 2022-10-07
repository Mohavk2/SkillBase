using Microsoft.Extensions.DependencyInjection;
using SkillBase.Models;
using SkillBase.ViewModels.Common;
using SkillBase.ViewModels.Factories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillBase.ViewModels.Schedule
{
    internal class DayOfWeekViewModel : BaseViewModel
    {
        IServiceProvider _serviceProvider;
        public DayOfWeekViewModel(IServiceProvider serviceProvider, DateTime date, IEnumerable<SkillTask> tasks)
        {
            _serviceProvider = serviceProvider;
            _date = date;
            Day = date.Day;
            DayOfWeek = date.DayOfWeek;

            var taskFactory = _serviceProvider.GetRequiredService<SkillTaskViewModelFactory>();

            foreach(var task in tasks.OrderBy(x => x.StartDate))
            {
                var taskVM = taskFactory.Create(task);
                TasksVMs.Add(taskVM);
            }
        }

        DateTime _date;
        public int TodayHighlightThickness
        {
            get
            {
                return _date.Date == DateTime.Today ? 1 : 0;
            }
        }

        public int Day { get; set; }
        public DayOfWeek DayOfWeek { get; set; }

        public ObservableCollection<SkillTaskViewModel> TasksVMs { get; set; } = new();
    }
}
