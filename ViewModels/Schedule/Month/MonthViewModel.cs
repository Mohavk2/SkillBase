using Microsoft.Extensions.DependencyInjection;
using SkillBase.Data;
using SkillBase.Extensions;
using SkillBase.ViewModels.Common;
using SkillBase.ViewModels.Factories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SkillBase.ViewModels.Schedule.Month
{
    internal class MonthViewModel : BaseViewModel
    {
        IServiceProvider _serviceProvider;
        public MonthViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _currentMonthStart = DateTime.Now.GetFirstDayOfMonth();
        }
        public async Task Init()
        {
            var db = _serviceProvider.GetRequiredService<MainDbContext>();
            var tasks = await db.GetMonthTasksAsync(DateTime.Now);

            var dayOfMonthFactory = _serviceProvider.GetRequiredService<DayOfMonthViewModelFactory>();
            ObservableCollection<DayOfMonthViewModel> dayOfMonthVMs = new();

            var currentMonthWeekStart = CurrentMonthStart.GetFirstDayOfWeek();
            for (DateTime i = currentMonthWeekStart; i < currentMonthWeekStart.AddDays(42); i = i.AddDays(1))
            {
                var dayTasks = tasks.Where(x => x.StartDate >= i && x.EndDate < i.AddDays(1)).ToList();
                var dayOfMonthVM = dayOfMonthFactory.Create(i, dayTasks, i.Month == CurrentMonthStart.Month);
                dayOfMonthVMs.Add(dayOfMonthVM);
            }
            DayOfMonthVMs = dayOfMonthVMs;
        }

        public ICommand Forward
        {
            get => new UICommand((parameter) =>
            {
                CurrentMonthStart = CurrentMonthStart.AddMonths(1);
                Task.Run(() => Init());
            });
        }

        public ICommand Back
        {
            get => new UICommand((parameter) =>
            {
                CurrentMonthStart = CurrentMonthStart.AddMonths(-1);
                Task.Run(() => Init());
            });
        }

        DateTime _currentMonthStart;
        public DateTime CurrentMonthStart
        {
            get => _currentMonthStart;
            set
            {
                _currentMonthStart = value;
                RaisePropertyChanged(nameof(CurrentMonthName));
                RaisePropertyChanged(nameof(CurrentYear));
            }
        }

        public string CurrentMonthName => CurrentMonthStart.ToMonthName();
        public int CurrentYear => CurrentMonthStart.Year;

        ObservableCollection<DayOfMonthViewModel> _dayOfMonthVMs = new();
        public ObservableCollection<DayOfMonthViewModel> DayOfMonthVMs
        {
            get => _dayOfMonthVMs;
            set
            {
                _dayOfMonthVMs = value;
                RaisePropertyChanged(nameof(DayOfMonthVMs));
            }
        }
    }
}
