using Microsoft.Extensions.DependencyInjection;
using SkillBase.Data;
using SkillBase.Extensions;
using SkillBase.ViewModels.Common;
using SkillBase.ViewModels.Factories;
using SkillBase.ViewModels.Schedule.Week;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SkillBase.ViewModels.Schedule.Year
{
    internal class YearViewModel : BaseViewModel
    {
        IServiceProvider _serviceProvider;
        public YearViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _currentYearStart = DateTime.Now.GetFirstDayOfYear();
        }
        public async Task Init()
        {
            using var db = _serviceProvider.GetRequiredService<MainDbContext>();
            var tasks = await db.GetYearTasksAsync(CurrentYearStart);

            var monthOfYearFactory = _serviceProvider.GetRequiredService<MonthOfYearViewModelFactory>();
            ObservableCollection<MonthOfYearViewModel> monthOfYearVMs = new();

            for (DateTime i = CurrentYearStart; i < CurrentYearStart.AddMonths(12); i = i.AddMonths(1))
            {
                var monthTasks = tasks.Where(x => (x.StartDate != null && x.EndDate != null) 
                && ((DateTime)x.StartDate).Month == i.Month 
                && ((DateTime)x.EndDate).Month == i.Month).ToList();

                var monthOfYearVM = monthOfYearFactory.Create(i, monthTasks ?? new());
                monthOfYearVMs.Add(monthOfYearVM);
            }
            MonthOfYearVMs = monthOfYearVMs;
        }

        public ICommand Forward => new UICommand((parameter)=>{
            CurrentYearStart = CurrentYearStart.AddYears(1);
            Task.Run(() => Init());
        });

        public ICommand Back => new UICommand((parameter) => {
            CurrentYearStart = CurrentYearStart.AddYears(-1);
            Task.Run(() => Init());
        });

        DateTime _currentYearStart;
        public DateTime CurrentYearStart
        {
            get => _currentYearStart;
            set
            {
                _currentYearStart = value;
                RaisePropertyChanged(nameof(CurrentYearStart));
            }
        }

        ObservableCollection<MonthOfYearViewModel> _monthOfYearVMs = new();
        public ObservableCollection<MonthOfYearViewModel> MonthOfYearVMs
        {
            get => _monthOfYearVMs;
            set
            {
                _monthOfYearVMs = value;
                RaisePropertyChanged(nameof(MonthOfYearVMs));
            }
        }
    }
}
