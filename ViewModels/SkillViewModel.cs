using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using SkillBase.Data;
using SkillBase.Models;
using SkillBase.ViewModels.Common;
using SkillBase.ViewModels.Factories;
using SkillBase.ViewModels.Skills;
using SkillBase.Views;
using SkillBase.Views.Skills;
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
    delegate void SkillCompletedChangedHandler(bool isCompleted);

    internal class SkillViewModel : BaseViewModel, IDisposable
    {
        public event DeleteSkillHandler? Deleted;
        public event SkillCompletedChangedHandler? CompletedChanged;

        IServiceProvider _serviceProvider;

        public SkillViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public SkillViewModel(Skill skill, IServiceProvider serviceProvider) : this(serviceProvider)
        {
            if (skill != null)
            {
                Id = skill.Id;
                _parentId = skill.ParentId;
                _name = skill.Name;
                _description = skill.Description ?? "Description...";
                _notes = skill.Notes ?? "Write your notes here...";

                if (skill.Children != null)
                {
                    foreach (var child in skill.Children.OrderByDescending(x => x.CreatedAt))
                    {
                        var skillVMFactory = _serviceProvider?.GetRequiredService<SkillViewModelFactory>();
                        if (skillVMFactory != null)
                        {
                            var skillVM = skillVMFactory.Create(child);
                            skillVM.ParentVM = this;
                            skillVM.Deleted += OnChildDeleted;
                            skillVM.CompletedChanged += OnChildCompletedChanged;
                            _childCount++;
                            if (skillVM.IsCompleted)
                            {
                                _completedChildCount++;
                            }
                            ChildrenVMs.Insert(0, skillVM);
                        }
                    }
                }
                if (skill.DayTasks != null)
                {
                    foreach (var task in skill.DayTasks)
                    {
                        var taskVMFactory = _serviceProvider.GetRequiredService<SkillTaskViewModelFactory>();
                        SkillTaskViewModel taskVM = taskVMFactory.Create(task);
                        taskVM.Deleted += OnTaskDeleted;
                        taskVM.CompletedChanged += OnTaskCompletedChanged;
                        _taskCount++;
                        if (taskVM.IsCompleted)
                        {
                            _completedTaskCount++;
                        }
                        TaskVMs.Insert(0, taskVM);
                    }
                }
                _isCompleted = ChildCount == CompletedChildCount && TaskCount == CompletedTaskCount;
            }
        }

        public void Dispose()
        {
            foreach (var vm in ChildrenVMs)
            {
                vm.Deleted -= OnChildDeleted;
                vm.CompletedChanged -= OnChildCompletedChanged;
                vm.Dispose();
            }
            ChildrenVMs.Clear();
            foreach (var vm in TaskVMs)
            {
                vm.Deleted -= OnTaskDeleted;
                vm.CompletedChanged -= OnTaskCompletedChanged;
                vm.Dispose();
            }
            TaskVMs.Clear();
        }

        void OnChildDeleted(SkillViewModel skillVM)
        {
            RemoveChildVM(skillVM);
            skillVM.Dispose();
        }

        private bool IsParent(int id, SkillViewModel target)
        {
            if (target.ParentVM == null) return false;
            return (target.ParentVM.Id == id || IsParent(id, target.ParentVM));
        }

        public int Id { get; private set; }

        int? _parentId;
        public int? ParentId
        {
            get => _parentId;
            set
            {
                _parentId = value;
                UpdateEntity(skill => skill.ParentId = _parentId);
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
                UpdateEntity(skill => skill.Name = _name);
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
                UpdateEntity(skill => skill.Description = _description);
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
                UpdateEntity(skill => skill.Notes = _notes);
                RaisePropertyChanged(nameof(Notes));
            }
        }
        public bool _isExpanded = false;
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                _isExpanded = value;
                RaisePropertyChanged(nameof(IsExpanded));
                foreach (var vm in ChildrenVMs)
                {
                    vm.IsExpanded = _isExpanded;
                }
            }
        }

        public SkillViewModel? ParentVM { get; set; }

        public ObservableCollection<SkillViewModel> ChildrenVMs { get; set; } = new();

        int _childCount = 0;
        public int ChildCount
        {
            get => _childCount;
            set
            {
                _childCount = value;
                IsCompleted = CompletedTaskCount == TaskCount && CompletedChildCount == ChildCount;
                RaisePropertyChanged(nameof(ChildCount));
            }
        }

        int _completedChildCount = 0;
        public int CompletedChildCount
        {
            get => _completedChildCount;
            set
            {
                _completedChildCount = value;
                IsCompleted = CompletedTaskCount == TaskCount && CompletedChildCount == ChildCount;
                RaisePropertyChanged(nameof(CompletedChildCount));
            }
        }

        public ICommand MoveHere
        {
            get => new UICommand((vm) =>
            {
                if (vm is SkillViewModel skillVM)
                {
                    if (skillVM.Id != Id && IsParent(skillVM.Id, this) == false && ChildrenVMs.Any(vm => vm.Id == skillVM.Id) == false)
                    {
                        if (skillVM.ParentVM != null)
                        {
                            skillVM.ParentVM.RemoveChildVM(skillVM);
                        }
                        else
                        {
                            var tree = _serviceProvider.GetRequiredService<SkillsViewModel>();
                            tree.RemoveChildVM(skillVM);
                        }
                        skillVM.ParentId = Id;
                        AddChildVM(skillVM);
                    }
                }

            });
        }
        public ICommand CreateChild
        {
            get => new UICommand((parameter) =>
            {
                Skill skill = new();
                skill.ParentId = Id;

                using var dbContext = _serviceProvider.GetRequiredService<MainDbContext>();
                dbContext.Skills.Add(skill);
                dbContext.SaveChanges();

                var skillVMFactory = _serviceProvider.GetRequiredService<SkillViewModelFactory>();
                var skillVM = skillVMFactory.Create(skill);

                AddChildVM(skillVM);
            });
        }
        public ICommand Delete
        {
            get => new UICommand((parameter) =>
            {
                using var db = _serviceProvider.GetRequiredService<MainDbContext>();
                var skill = db.Skills.Find(Id);
                if (skill != null)
                {
                    db.Remove(skill);
                    db.SaveChanges();
                    Deleted?.Invoke(this);
                }
            });
        }
        public void AddChildVM(SkillViewModel childVM)
        {
            childVM.ParentVM = this;
            childVM.Deleted += OnChildDeleted;
            childVM.CompletedChanged += OnChildCompletedChanged;
            ChildCount++;
            if (childVM.IsCompleted)
            {
                CompletedChildCount++;
            }
            ChildrenVMs.Insert(0, childVM);
        }
        public void RemoveChildVM(SkillViewModel childVM)
        {
            childVM.ParentVM = null;
            childVM.Deleted -= OnChildDeleted;
            childVM.CompletedChanged -= OnChildCompletedChanged;
            ChildCount--;
            if (childVM.IsCompleted)
            {
                CompletedChildCount--;
            }
            ChildrenVMs.Remove(childVM);
        }

        private void OnChildCompletedChanged(bool isCompleted)
        {
            if (isCompleted) CompletedChildCount++;
            else CompletedChildCount--;
        }

        bool _isCompleted;
        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                var old = _isCompleted;
                _isCompleted = value;
                if (_isCompleted != old)
                {
                    CompletedChanged?.Invoke(_isCompleted);
                }
                RaisePropertyChanged(nameof(IsCompleted));
            }
        }

        public ObservableCollection<SkillTaskViewModel> TaskVMs { get; set; } = new();

        private void OnTaskDeleted(SkillTaskViewModel taskVM)
        {
            RemoveTaskVM(taskVM);
        }

        public ICommand CreateDayTask
        {
            get => new UICommand((paremeter) =>
            {
                SkillTask task = new() { SkillId = Id };
                var db = _serviceProvider.GetRequiredService<MainDbContext>();
                db.Add<SkillTask>(task);
                db.SaveChanges();

                var taskVMFactory = _serviceProvider.GetRequiredService<SkillTaskViewModelFactory>();
                SkillTaskViewModel taskVM = taskVMFactory.Create(task);
                AddTaskVM(taskVM);
            });
        }

        public void AddTaskVM(SkillTaskViewModel taskVM)
        {
            taskVM.Deleted += OnTaskDeleted;
            taskVM.CompletedChanged += OnTaskCompletedChanged;
            TaskCount++;
            if (taskVM.IsCompleted)
            {
                CompletedTaskCount++;
            }
            TaskVMs.Insert(0, taskVM);
        }
        public void RemoveTaskVM(SkillTaskViewModel taskVM)
        {
            taskVM.Deleted -= OnTaskDeleted;
            taskVM.CompletedChanged -= OnTaskCompletedChanged;
            TaskCount--;
            if (taskVM.IsCompleted)
            {
                CompletedTaskCount--;
            }
            TaskVMs.Remove(taskVM);
        }

        private void OnTaskCompletedChanged(bool isCompleted)
        {
            if (isCompleted) CompletedTaskCount++;
            else CompletedTaskCount--;
        }

        int _taskCount = 0;
        public int TaskCount 
        { 
            get => _taskCount;
            set
            {
                _taskCount = value;
                IsCompleted = CompletedTaskCount == TaskCount && CompletedChildCount == ChildCount;
                RaisePropertyChanged(nameof(TaskCount));
            }
        }
        int _completedTaskCount = 0;
        public int CompletedTaskCount
        {
            get => _completedTaskCount;
            set
            {
                _completedTaskCount = value;
                IsCompleted = CompletedTaskCount == TaskCount && CompletedChildCount == ChildCount;
                RaisePropertyChanged(nameof(CompletedTaskCount));
            }
        }

        int _number = 0;
        public int Number
        {
            get => _number;
            set
            {
                _number = value;
                RaisePropertyChanged(nameof(Number));
            }
        }

        double _height = 0;
        public double Height
        {
            get => _height;
            set
            {
                _height = value;
                RaisePropertyChanged(nameof(Height));
            }
        }

        void UpdateEntity(Action<Skill> setter)
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
    }
}
