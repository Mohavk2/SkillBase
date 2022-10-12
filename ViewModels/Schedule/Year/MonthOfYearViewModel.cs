using Microsoft.Extensions.DependencyInjection;
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

namespace SkillBase.ViewModels.Schedule.Year
{
    internal class MonthOfYearViewModel : BaseViewModel, IDisposable
    {
        IServiceProvider _serviceProvider;
        public MonthOfYearViewModel(IServiceProvider serviceProvider, DateTime date, IEnumerable<SkillTask> tasks)
        {
            _serviceProvider = serviceProvider;
            _date = date;

            var dayOfYearFactory = _serviceProvider.GetRequiredService<DayOfYearViewModelFactory>();
            var dayOfYearVMs = new ObservableCollection<DayOfYearViewModel>();

            var currentMonthWeekStart = date.GetFirstDayOfMonth().GetFirstDayOfWeek();
            for (DateTime i = currentMonthWeekStart; i < currentMonthWeekStart.AddDays(42); i = i.AddDays(1))
            {
                var dayTasks = tasks.Where(x => x.StartDate >= i && x.EndDate < i.AddDays(1)).ToList();
                var dayOfYearVM = dayOfYearFactory.Create(i, dayTasks, i.Month == Date.Month);
                dayOfYearVMs.Add(dayOfYearVM);
            }
            DayOfYearVMs = dayOfYearVMs;
        }

        public void Dispose()
        {
            foreach(var vm in DayOfYearVMs)
            {
                vm.Dispose();
            }
            DayOfYearVMs.Clear();
        }

        DateTime _date;
        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                RaisePropertyChanged(nameof(MonthName));
            }
        }

        public string MonthName
        {
            get => Date.ToMonthName();
        }

        ObservableCollection<DayOfYearViewModel> _dayOfYearVMs = new();
        public ObservableCollection<DayOfYearViewModel> DayOfYearVMs
        {
            get => _dayOfYearVMs;
            set
            {
                _dayOfYearVMs = value;
                RaisePropertyChanged(nameof(DayOfYearVMs));
            }
        }
    }
}
