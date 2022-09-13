using Microsoft.Extensions.DependencyInjection;
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

    internal class SkillViewModel : BaseViewModel, IDisposable
    {
        public event DeleteSkillHandler? OnDelete;

        IServiceProvider _serviceProvider;

        Skill _skill;

        public SkillViewModel(Skill skill, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _skill = skill;
            var chilldren = skill.Children ?? new();
            foreach (var child in chilldren)
            {
                var skillFactory = _serviceProvider?.GetRequiredService<SkillViewModelFactory>();
                if(skillFactory != null)
                {
                    var skillVM = skillFactory.Create(child);
                    skillVM.OnDelete += Delete;
                    SkillVMs.Add(skillVM);
                }
            }
            foreach (var reference in _skill.References)
            {
                var referenceVMFactory = _serviceProvider.GetRequiredService<ReferenceViewModelFactory>();
                ReferenceUrlViewModel referenceVM = referenceVMFactory.Create(reference);
                referenceVM.OnDelete += DeleteReference;
                References.Add(referenceVM);
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

        void DeleteReference(ReferenceUrlViewModel referenceVM)
        {
            References.Remove(referenceVM);
            using var dbContext = _serviceProvider.GetRequiredService<MainDbContext>();
            _skill = dbContext.Skills.Find(_skill.Id);
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

        public ICommand CreateReference
        {
            get => new UICommand((paremeter) =>
            {
                ReferenceUrl reference = new();
                _skill.References.Add(reference);
                UpdateModel();

                var referenceVMFactory = _serviceProvider.GetRequiredService<ReferenceViewModelFactory>();
                ReferenceUrlViewModel referenceVM = referenceVMFactory.Create(reference);
                referenceVM.OnDelete += DeleteReference;
                References.Add(referenceVM);
            });
        }

        public ICommand CreateSkill
        {
            get => new UICommand((parameter) =>
            {
                Skill skill = new();
                skill.ParentId = _skill.Id;

                using var dbContext = _serviceProvider.GetRequiredService<MainDbContext>();
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
            get => _skill.Notes ?? "Write your notes here...";
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

        public ObservableCollection<ReferenceUrlViewModel> References { get; set; } = new();

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

        public ObservableCollection<SkillViewModel> SkillVMs { get; set; } = new();
    }
}
