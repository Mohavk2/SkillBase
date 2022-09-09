using Microsoft.Extensions.DependencyInjection;
using SkillBase.Data;
using SkillBase.Models;
using SkillBase.ViewModels.Common;
using SkillBase.ViewModels.Factories;
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
    delegate void DeleteHandler(SkillViewModel skillVM);

    internal class SkillViewModel : BaseViewModel, IDisposable
    {
        public event DeleteHandler? OnDelete;

        IServiceProvider _serviceProvider;

        Skill _skill;

        public SkillViewModel(Skill skill, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _skill = skill;
            Description = skill.Description ?? String.Empty; ;
            Notes = skill.Notes ?? String.Empty;
            var chilldren = skill.Children ?? new();
            foreach (var child in chilldren)
            {
                var skillFactory = _serviceProvider?.GetRequiredService<SkillViewModelFactory>();
                var skillVM = skillFactory.Create(child);
                skillVM.OnDelete += Delete;
                SkillVMs.Add(skillVM);
            }
        }
        public void Dispose()
        {
            foreach (var vm in SkillVMs)
            {
                vm.Dispose();
            }
            using var dbContext = _serviceProvider.GetRequiredService<MainDbContext>();
            dbContext.Skills.Remove(_skill);
            dbContext.SaveChanges();
        }

        void Delete(SkillViewModel skillVM)
        {
            skillVM.Dispose();
            SkillVMs.Remove(skillVM);
            RaisePropertyChanged(nameof(HasChildren));
        }
        void UpdateModel()
        {
            using var dbContext = _serviceProvider.GetRequiredService<MainDbContext>();
            dbContext.Skills.Update(_skill);
            dbContext.SaveChanges();
        }

        public ICommand CreateSkill
        {
            get => new UICommand((parameter) =>
            {
                using var dbContext = _serviceProvider.GetRequiredService<MainDbContext>();
                Skill skill = new();
                skill.ParentId = _skill.Id;
                dbContext.Skills.Add(skill);
                dbContext.SaveChanges();

                var skillVMFactory = _serviceProvider.GetRequiredService<SkillViewModelFactory>();
                var skillVM = skillVMFactory.Create(skill);
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

        public int Id => _skill.Id;
        public string Name
        {
            get => _skill.Name;
            set 
            {
                _skill.Name = value;
                UpdateModel();
                RaisePropertyChanged(nameof(Name));
            }
        }
        public string Description
        {
            get => _skill.Description ?? String.Empty;
            set
            {
                _skill.Description = value;
                UpdateModel();
                RaisePropertyChanged(nameof(Description));
            }
        }
        public string Notes
        {
            get => _skill.Notes ?? String.Empty;
            set
            {
                _skill.Notes = value;
                UpdateModel();
                RaisePropertyChanged(nameof(Notes));
            }
        }
        public string ImagePath
        {
            get => _skill?.ImagePath ?? Path.Combine(Directory.GetCurrentDirectory(), "resources/img/skill.png");
            set
            {
                _skill.ImagePath = value;
                UpdateModel();
                RaisePropertyChanged(nameof(ImagePath));
            }
        }
        public ObservableCollection<ReferenceUrl> _references = new();
        public ObservableCollection<ReferenceUrl> References
        {
            get { return _references; }
            set
            {
                _references = value;
                RaisePropertyChanged(nameof(References));
            }
        }
        public bool IsCompleted
        {
            get => _skill.IsCompleted;
            set
            {
                _skill.IsCompleted = value;
                UpdateModel();
                RaisePropertyChanged(nameof(IsCompleted));
            }
        }
        public bool HasChildren
        {
            get => SkillVMs.Count > 0;
        }
        public ObservableCollection<SkillViewModel> _skillVMs = new();
        public ObservableCollection<SkillViewModel> SkillVMs
        {
            get => _skillVMs;
            set
            {
                _skillVMs = value;
                RaisePropertyChanged(nameof(SkillVMs));
            }
        }
    }
}
