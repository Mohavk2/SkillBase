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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SkillBase.ViewModels.Schedule.Week
{
    internal class WeekViewModel : BaseViewModel
    {
        IServiceProvider _serviceProvider;
        public WeekViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _currentWeekStart = DateTime.Today.GetFirstDayOfWeek();
        }

        public async Task Init()
        {
            using var db = _serviceProvider.GetRequiredService<MainDbContext>();
            var tasks = await db.GetWeekTasksAsync(CurrentWeekStart);

            var dayOfWeekFactory = _serviceProvider.GetRequiredService<DayOfWeekViewModelFactory>();
            ObservableCollection<DayOfWeekViewModel> dayOfWeekVMs = new();
            for (DateTime i = CurrentWeekStart; i < CurrentWeekStart.AddDays(7); i = i.AddDays(1))
            {
                var dayTasks = tasks.Where(x => x.StartDate >= i && x.EndDate < i.AddDays(1)).ToList();
                var dayOfWeekVM = dayOfWeekFactory.Create(i, dayTasks ?? new());
                dayOfWeekVMs.Add(dayOfWeekVM);
            }
            DayOfWeekVMs = dayOfWeekVMs;
        }

        public ICommand Forward
        {
            get => new UICommand((parameter) =>
            {
                CurrentWeekStart = CurrentWeekStart.AddDays(7);
                Task.Run(() => Init());
            });
        }

        public ICommand Back
        {
            get => new UICommand((parameter) =>
            {
                CurrentWeekStart = CurrentWeekStart.AddDays(-7);
                Task.Run(() => Init());
            });
        }

        DateTime _currentWeekStart;
        public DateTime CurrentWeekStart
        {
            get => _currentWeekStart;
            set
            {
                _currentWeekStart = value;
                RaiseWeekChanged();
            }
        }

        void RaiseWeekChanged()
        {
            RaisePropertyChanged(nameof(DayFrom));
            RaisePropertyChanged(nameof(DayOfWeekFrom));
            RaisePropertyChanged(nameof(MonthFrom));
            RaisePropertyChanged(nameof(YearFrom));
            RaisePropertyChanged(nameof(DayTo));
            RaisePropertyChanged(nameof(DayOfWeekTo));
            RaisePropertyChanged(nameof(MonthTo));
            RaisePropertyChanged(nameof(YearTo));
        }

        public int DayFrom => _currentWeekStart.Day;
        public DayOfWeek DayOfWeekFrom => _currentWeekStart.DayOfWeek;
        public string MonthFrom => _currentWeekStart.ToString("MMMM");
        public int YearFrom => _currentWeekStart.Year;
        public int DayTo => _currentWeekStart.AddDays(6).Day;
        public DayOfWeek DayOfWeekTo => _currentWeekStart.AddDays(6).DayOfWeek;
        public string MonthTo => _currentWeekStart.AddDays(6).ToString("MMMM");
        public int YearTo => _currentWeekStart.AddDays(6).Year;

        ObservableCollection<DayOfWeekViewModel> _dayOfWeekVMs = new();
        public ObservableCollection<DayOfWeekViewModel> DayOfWeekVMs
        {
            get => _dayOfWeekVMs;
            set
            {
                _dayOfWeekVMs = value;
                RaisePropertyChanged(nameof(DayOfWeekVMs));
            }
        }
    }
}
