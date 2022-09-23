using Microsoft.Extensions.DependencyInjection;
using SkillBase.Data;
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
    delegate void DeleteDayTaskHandler(DayTaskViewModel task);

    internal class DayTaskViewModel : BaseViewModel
    {
        public event DeleteDayTaskHandler? OnDelete;

        IServiceProvider _serviceProvider;

        public DayTaskViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public DayTaskViewModel(DayTask task, IServiceProvider serviceProvider) : this(serviceProvider)
        {
            if(task != null)
            {
                Id = task.Id;
                Name = task.Name;
                Description = task.Description;
                StartDate = task.StartDate;
                IsCompleted = task.IsCompleted;
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
                var task = dbContext.Find<DayTask>(Id);
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
                Link link = new() { DayTaskId = Id };
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
        DateTime? _startDate;
        public DateTime? StartDate 
        {
            get => _startDate;
            set
            {
                _startDate = value;
                Update(task => task.StartDate = _startDate);
                RaisePropertyChanged(nameof(StartDate));
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

        public int SkillId { get; private set; }

        void Update(Action<DayTask> setter)
        {
            using var db = _serviceProvider.GetRequiredService<MainDbContext>();
            var entity = db.Find<DayTask>(Id);
            if (entity != null)
            {
                setter(entity);
                db.Update<DayTask>(entity);
                db.SaveChanges();
            }
        }
    }
}
