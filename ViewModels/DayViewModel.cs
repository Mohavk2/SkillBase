using SkillBase.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SkillBase.ViewModels
{
    internal class DayViewModel : BaseViewModel
    {

        public DayViewModel()
        {

        }

        public ICommand Forward
        {
            get => new UICommand((parameter) =>
            {
                CurrentDayTime = CurrentDayTime.AddDays(1);
            });
        }

        public ICommand Back
        {
            get => new UICommand((parameter) =>
            {
                CurrentDayTime = CurrentDayTime.AddDays(-1);
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

        List<SkillTaskViewModel> _tasksVMs = new();
        public List<SkillTaskViewModel> TasksVMs
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
