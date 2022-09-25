using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using SkillBase.Data;
using SkillBase.Models;
using SkillBase.ViewModels.Common;
using SkillBase.ViewModels.Factories;
using SkillBase.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SkillBase.ViewModels
{
    delegate void DeleteSkillHandler(SkillViewModel skillVM);

    internal class SkillViewModel : BaseViewModel
    {
        public event DeleteSkillHandler? OnDelete;

        IServiceProvider _serviceProvider;

        public SkillViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public SkillViewModel(Skill skill, SkillViewModel? parentVM, IServiceProvider serviceProvider) : this(serviceProvider)
        {
            if (skill != null)
            {
                Id = skill.Id;
                ParentId = skill.ParentId;
                ParentVM = parentVM;
                Name = skill.Name;
                Description = skill.Description ?? "Description...";
                Notes = skill.Notes ?? "Write your notes here...";
                IsCompleted = skill.IsCompleted;

                if (skill.Children != null)
                {
                    foreach (var child in skill.Children)
                    {
                        var skillVMFactory = _serviceProvider?.GetRequiredService<SkillViewModelFactory>();
                        if (skillVMFactory != null)
                        {
                            var skillVM = skillVMFactory.Create(child, this);
                            skillVM.OnDelete += Delete;
                            SkillVMs.Add(skillVM);
                        }
                    }
                }
                if (skill.DayTasks != null)
                {
                    foreach (var task in skill.DayTasks)
                    {
                        var taskVMFactory = _serviceProvider.GetRequiredService<DayTaskViewModelFactory>();
                        SkillTaskViewModel taskVM = taskVMFactory.Create(task);
                        taskVM.OnDelete += DeleteTask;
                        Tasks.Add(taskVM);
                    }
                }
            }
        }

        public void Dispose()
        {
            foreach (var vm in SkillVMs)
            {
                vm.Dispose();
            }
            using var dbContext = _serviceProvider.GetRequiredService<MainDbContext>();
            var skill = dbContext.Skills.Find(Id);
            if (skill != null)
            {
                dbContext.Skills.Remove(skill);
                dbContext.SaveChanges();
            }
        }

        void Delete(SkillViewModel skillVM)
        {
            skillVM.Dispose();
            SkillVMs.Remove(skillVM);
            RaisePropertyChanged(nameof(HasChildren));
        }

        public ICommand MoveHere
        {
            get => new UICommand((vm) =>
            {
                if (vm is SkillViewModel child)
                {
                    if (child.Id != Id && IsParent(child.Id, this) == false)
                    {
                        if (child.ParentVM != null)
                        {
                            child.ParentVM.RemoveChild(child);
                        }
                        else
                        {
                            var tree = _serviceProvider.GetRequiredService<SkillsViewModel>();
                            tree.RemoveChild(child);
                        }
                        AddChild(child);
                    }
                }

            });
        }

        private bool IsParent(int id, SkillViewModel target)
        {
            if (target.ParentVM == null) return false;
            return (target.ParentVM.Id == id || IsParent(id, target.ParentVM));
        }

        public ICommand CreateDayTask
        {
            get => new UICommand((paremeter) =>
            {
                SkillTask task = new() { SkillId = Id };
                var db = _serviceProvider.GetRequiredService<MainDbContext>();
                db.Add<SkillTask>(task);
                db.SaveChanges();

                var taskVMFactory = _serviceProvider.GetRequiredService<DayTaskViewModelFactory>();
                SkillTaskViewModel taskVM = taskVMFactory.Create(task);
                taskVM.OnDelete += DeleteTask;
                Tasks.Add(taskVM);
            });
        }

        public ICommand CreateSkill
        {
            get => new UICommand((parameter) =>
            {
                Skill skill = new();
                skill.ParentId = Id;

                using var dbContext = _serviceProvider.GetRequiredService<MainDbContext>();
                dbContext.Skills.Add(skill);
                dbContext.SaveChanges();

                var skillVMFactory = _serviceProvider.GetRequiredService<SkillViewModelFactory>();
                var skillVM = skillVMFactory.Create(skill, this);
                skillVM.OnDelete += Delete;
                SkillVMs.Add(skillVM);
                RaisePropertyChanged(nameof(HasChildren));
            });
        }
        public ICommand DeleteSkill
        {
            get => new UICommand((parameter) =>
            {
                OnDelete?.Invoke(this);
            });
        }

        public int Id { get; private set; }

        int? _parentId;
        public int? ParentId
        {
            get => _parentId;
            set
            {
                _parentId = value;
                Update(skill => skill.ParentId = _parentId);
                RaisePropertyChanged(nameof(ParentId));
            }
        }

        string _name = "New skill";
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                Update(skill => skill.Name = _name);
                RaisePropertyChanged(nameof(Name));
            }
        }
        string _description = "Description...";
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                Update(skill => skill.Description = _description);
                RaisePropertyChanged(nameof(Description));
            }
        }
        string _notes = "Write your notes here...";
        public string Notes
        {
            get => _notes;
            set
            {
                _notes = value;
                Update(skill => skill.Notes = _notes);
                RaisePropertyChanged(nameof(Notes));
            }
        }

        public ObservableCollection<SkillTaskViewModel> Tasks { get; set; } = new();
        private void DeleteTask(SkillTaskViewModel taskVM)
        {
            Tasks.Remove(taskVM);
        }

        bool _isCompleted = false;
        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                _isCompleted = value;
                Update(skill => skill.IsCompleted = _isCompleted);
                RaisePropertyChanged(nameof(IsCompleted));
            }
        }

        public void SetParent(SkillViewModel? parentVM)
        {
            if (parentVM == null)
            {
                ParentId = null;
                ParentVM = null;
            }
            else
            {
                ParentId = parentVM.Id;
                ParentVM = parentVM;
            }
        }

        public void AddChild(SkillViewModel childVM)
        {
            childVM.SetParent(this);
            if (SkillVMs.Any(vm => vm.Id == childVM.Id) == false)
            {
                SkillVMs.Add(childVM);
            }
            RaisePropertyChanged(nameof(HasChildren));
        }

        public void RemoveChild(SkillViewModel childVM)
        {
            SkillVMs.Remove(childVM);
            RaisePropertyChanged(nameof(HasChildren));
        }

        void Update(Action<Skill> setter)
        {
            using var db = _serviceProvider.GetRequiredService<MainDbContext>();
            var entity = db.Find<Skill>(Id);
            if (entity != null)
            {
                setter(entity);
                db.Update<Skill>(entity);
                db.SaveChanges();
            }
        }

        public bool HasChildren
        {
            get => SkillVMs.Count > 0;
        }

        public SkillViewModel? ParentVM { get; set; }

        public ObservableCollection<SkillViewModel> SkillVMs { get; set; } = new();
    }
}
