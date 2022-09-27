using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
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

    internal class SkillTaskViewModel : BaseViewModel
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
                _startDateTime = task.StartDate;
                _endDateTime = task.EndDate;
                _isCompleted = task.IsCompleted;
                SkillId = task.SkillId;

                if (task.Links != null)
                {
                    foreach (var link in task.Links)
                    {
                        var linkVMFactory = _serviceProvider.GetRequiredService<LinkViewModelFactory>();
                        LinkViewModel linkVM = linkVMFactory.Create(link);
                        linkVM.OnDelete += DeleteLink;
                        Links.Add(linkVM);
                    }
                }
            }
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
                Links.Add(linkVM);
            });
        }
        void DeleteLink(LinkViewModel linkVM)
        {
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
        DateTime? _startDateTime;
        public DateTime? StartDateTime
        {
            get => _startDateTime;
            set
            {
                if (value is DateTime v && value != StartDateTime)
                {
                    if (EndDateTime is DateTime edt)
                    {
                        if (v > edt || v.Date < edt.Date)
                        {
                            CorrectEndDateTime(v);
                        }
                        if (CheckIfTimeFree(v, v))
                        {
                            _startDateTime = v;
                            Update(task => task.StartDate = _startDateTime);
                            RaisePropertyChanged(nameof(StartDateTime));
                            DateError = null;
                        }
                    }
                    else if(CheckIfTimeFree(v, EndDateTime ?? v))
                    {
                        _startDateTime = v;
                        Update(task => task.StartDate = _startDateTime);
                        RaisePropertyChanged(nameof(StartDateTime));
                        DateError = null;
                    }
                }
            }
        }
        void CorrectEndDateTime(DateTime value)
        {
            if (CheckIfTimeFree(value, value))
            {
                _endDateTime = value;
                Update(task => task.EndDate = _endDateTime);
                RaisePropertyChanged(nameof(EndDateTime));
                DateError = null;
            }
        }
        DateTime? _endDateTime;
        public DateTime? EndDateTime
        {
            get => _endDateTime;
            set
            {
                if (value is DateTime v && value != EndDateTime)
                {
                    if (StartDateTime is DateTime sdt)
                    {
                        if (v < sdt)
                        {
                            v = sdt;
                        }
                        else if (v.Date > sdt.Date)
                        {
                            v = new DateTime(sdt.Year, sdt.Month, sdt.Day, 23, 59, 59);
                        }
                    }
                    if (CheckIfTimeFree(StartDateTime ?? v, v))
                    {
                        _endDateTime = v;
                        Update(task => task.EndDate = _endDateTime);
                        RaisePropertyChanged(nameof(EndDateTime));
                        DateError = null;
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
            if (collisions.Count == 0) return true;
            PrintBusyHours(tasksExceptCurrent);
            return false;
        }
        void PrintBusyHours(IEnumerable<SkillTask> tasks)
        {
            if(tasks.Count() == 0) return;
            var day = tasks.First().StartDate?.ToString("dd MMM yyyy");
            string error = "Busy hours "+ day + "\r";
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
        public int Minutes
        {
            get
            {
                if (EndDateTime is DateTime end && StartDateTime is DateTime start)
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
