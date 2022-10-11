using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SkillBase.Data;
using SkillBase.Extensions;
using SkillBase.Models;
using SkillBase.ViewModels.Common;
using SkillBase.ViewModels.Factories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SkillBase.ViewModels.Schedule.Day
{
    internal class DayViewModel : BaseViewModel
    {
        IServiceProvider _serviceProvider;
        public DayViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Init()
        {
            using var db = _serviceProvider.GetRequiredService<MainDbContext>();

            var tasks = await db.GetDayTasksAsync(CurrentDayTime);

            var taskFactory = _serviceProvider.GetRequiredService<SkillTaskViewModelFactory>();
            var taskVMs = new ObservableCollection<SkillTaskViewModel>();
            foreach (var task in tasks)
            {
                var tvm = taskFactory.Create(task);
                taskVMs.Add(tvm);
            }
            TasksVMs = taskVMs;
        }

        public ICommand Forward
        {
            get => new UICommand((parameter) =>
            {
                CurrentDayTime = CurrentDayTime.AddDays(1);
                Task.Run(() => Init());
            });
        }

        public ICommand Back
        {
            get => new UICommand((parameter) =>
            {
                CurrentDayTime = CurrentDayTime.AddDays(-1);
                Task.Run(() => Init());
            });
        }

        DateTime _currentDayTime = DateTime.Now;
        public DateTime CurrentDayTime
        {
            get => _currentDayTime;
            set
            {
                _currentDayTime = value;
                RaisePropertyChanged(nameof(Day));
                RaisePropertyChanged(nameof(DayOfWeek));
                RaisePropertyChanged(nameof(Month));
                RaisePropertyChanged(nameof(Year));
            }
        }
        public int Day => _currentDayTime.Day;
        public DayOfWeek DayOfWeek => _currentDayTime.DayOfWeek;
        public string Month => _currentDayTime.ToString("MMMM");
        public int Year => _currentDayTime.Year;

        ObservableCollection<SkillTaskViewModel> _tasksVMs = new();
        public ObservableCollection<SkillTaskViewModel> TasksVMs
        {
            get => _tasksVMs;
            set
            {
                _tasksVMs = value;
                RaisePropertyChanged(nameof(TasksVMs));
            }
        }
    }
}
