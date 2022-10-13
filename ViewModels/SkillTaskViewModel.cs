using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SkillBase.Data;
using SkillBase.Extensions;
using SkillBase.Models;
using SkillBase.ViewModels.Common;
using SkillBase.ViewModels.Factories;
using SkillBase.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Shapes;

namespace SkillBase.ViewModels
{
    delegate void DeleteSkillTaskHandler(SkillTaskViewModel task);

    internal class SkillTaskViewModel : BaseViewModel, IDisposable
    {
        public event DeleteSkillTaskHandler? OnDelete;

        IServiceProvider _serviceProvider;

        public SkillTaskViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public SkillTaskViewModel(SkillTask task, IServiceProvider serviceProvider) : this(serviceProvider)
        {
            if (task != null)
            {
                Id = task.Id;
                _name = task.Name;
                SkillName = task.Skill?.Name ?? "";
                _description = task.Description;
                _date = task.StartDate;
                _startTime = task.StartDate;
                _endTime = task.EndDate;
                _isCompleted = task.IsCompleted;
                SkillId = task.SkillId;

                if (task.Links != null)
                {
                    foreach (var link in task.Links.OrderByDescending(x => x.CreatedAt))
                    {
                        var linkVMFactory = _serviceProvider.GetRequiredService<LinkViewModelFactory>();
                        LinkViewModel linkVM = linkVMFactory.Create(link);
                        linkVM.OnDelete += DeleteLink;
                        Links.Add(linkVM);
                    }
                }
            }
        }
        public void Dispose()
        {
            foreach(var vm in Links)
            {
                vm.OnDelete -= DeleteLink;
            }
            Links.Clear();
        }

        public ICommand Delete
        {
            get => new UICommand((parameter) =>
            {
                using var dbContext = _serviceProvider.GetRequiredService<MainDbContext>();
                var task = dbContext.Find<SkillTask>(Id);
                if (task != null)
                {
                    dbContext.Remove(task);
                    dbContext.SaveChanges();
                    OnDelete?.Invoke(this);
                }
            });
        }

        public ObservableCollection<LinkViewModel> Links { get; set; } = new();
        public ICommand CreateLink
        {
            get => new UICommand((paremeter) =>
            {
                Link link = new() { SkillTaskId = Id };
                var db = _serviceProvider.GetRequiredService<MainDbContext>();
                db.Add<Link>(link);
                db.SaveChanges();

                var linkVMFactory = _serviceProvider.GetRequiredService<LinkViewModelFactory>();
                LinkViewModel linkVM = linkVMFactory.Create(link);
                linkVM.OnDelete += DeleteLink;
                Links.Insert(0, linkVM);
            });
        }
        void DeleteLink(LinkViewModel linkVM)
        {
            linkVM.OnDelete -=DeleteLink;
            Links.Remove(linkVM);
        }

        public int Id { get; private set; } = 0;

        string _name = "New Task";
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                Update(task => task.Name = _name);
                RaisePropertyChanged(nameof(Name));
            }
        }
        string? _description = "Description...";
        public string? Description
        {
            get => _description;
            set
            {
                _description = value;
                Update(task => task.Description = _description);
                RaisePropertyChanged(nameof(Description));
            }
        }
        public bool IsTimeSet => (Date == null || StartTime == null || EndTime == null);

        DateTime? _date;
        public DateTime? Date
        {
            get {
                return _date;
            }
            set
            {
                if (value is DateTime v)
                {
                    _date = v;
                    RaisePropertyChanged(nameof(Date));
                    SaveTime();
                }
            }
        }
        DateTime? _startTime;
        public DateTime? StartTime
        {
            get => _startTime;
            set
            {
                if (value is DateTime v)
                {
                    if(v != _startTime && (_endTime == null || v < _endTime))
                    {
                        _startTime = v;
                        RaisePropertyChanged(nameof(StartTime));
                        SaveTime();
                    }
                }
            }
        }
        public string StartTimeShort => StartTime != null ? ((DateTime)StartTime).ToShortTimeString() : "";
        DateTime ? _endTime;
        public DateTime? EndTime
        {
            get => _endTime;
            set
            {
                if (value is DateTime v)
                {
                    if (v != _endTime && (_startTime == null || v > _startTime))
                    {
                        _endTime = v;
                        RaisePropertyChanged(nameof(EndTime));
                        SaveTime();
                    }
                }
            }
        }
        public string EndTimeShort => EndTime != null ? ((DateTime)EndTime).ToShortTimeString() : "";
        void SaveTime()
        {
            if (_date is DateTime d && _startTime is DateTime st && _endTime is DateTime et)
            {
                DateTime start = d.SetTime(st);
                DateTime end = d.SetTime(et);

                if(CheckIfTimeFree(start, end))
                {
                    using var db = _serviceProvider.GetRequiredService<MainDbContext>();
                    var task = db.Find<SkillTask>(Id);
                    if (task != null)
                    {
                        task.StartDate = start;
                        task.EndDate = end;
                        db.SaveChanges();
                        RaisePropertyChanged(nameof(IsTimeSet));
                    }
                }
            }
        }
        bool CheckIfTimeFree(DateTime start, DateTime end)
        {
            using var db = _serviceProvider.GetRequiredService<MainDbContext>();
            var dayTasks = db.GetDayTasks(start);
            var tasksExceptCurrent = dayTasks.Where(x => x.Id != Id).ToList();
            var collisions = tasksExceptCurrent.Where(x => !(x.StartDate >= end || x.EndDate <= start)).ToList();

            if (collisions.Count == 0) 
            {
                DateError = null;
                return true;
            }

            PrintBusyHours(tasksExceptCurrent.OrderBy(x=> x.StartDate));
            return false;
        }
        void PrintBusyHours(IEnumerable<SkillTask> tasks)
        {
            if (tasks.Count() == 0) return;
            var day = tasks.First().StartDate?.ToString("dd MMM yyyy");
            string error = "Busy hours " + day + "\r";
            foreach (var task in tasks)
            {
                error += "\u231A";
                if (task.StartDate is DateTime sdt)
                {
                    error += sdt.ToString("HH:mm") + " - ";
                }
                if (task.EndDate is DateTime edt)
                {
                    error += edt.ToString("HH:mm") + "\r";
                }
            }
            DateError = error;
        }
        public string? _dateError;
        public string? DateError
        {
            get => _dateError;
            set
            {
                _dateError = value;
                RaisePropertyChanged(nameof(DateError));
            }
        }
        public int Minutes
        {
            get
            {
                if (EndTime is DateTime end && StartTime is DateTime start)
                {
                    return (int)(end - start).TotalMinutes;
                }
                return 0;
            }
        }

        bool _isCompleted = false;
        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                _isCompleted = value;
                Update(task => task.IsCompleted = _isCompleted);
                RaisePropertyChanged(nameof(IsCompleted));
            }
        }

        public string SkillName { get; private set; } = "";

        public int SkillId { get; private set; }

        void Update(Action<SkillTask> setter)
        {
            using var db = _serviceProvider.GetRequiredService<MainDbContext>();
            var entity = db.Find<SkillTask>(Id);
            if (entity != null)
            {
                setter(entity);
                db.Update<SkillTask>(entity);
                db.SaveChanges();
            }
        }
    }
}
