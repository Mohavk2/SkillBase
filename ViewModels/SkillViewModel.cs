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

    internal class SkillViewModel : BaseViewModel
    {
        public event DeleteSkillHandler? OnDelete;

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
                Name = skill.Name;
                Description = skill.Description ?? "Description...";
                Notes = skill.Notes ?? "Write your notes here...";
                ImagePath = skill.ImagePath = "";
                IsCompleted = skill.IsCompleted;

                if (skill.Children != null)
                {
                    foreach (var child in skill.Children)
                    {
                        var skillVMFactory = _serviceProvider?.GetRequiredService<SkillViewModelFactory>();
                        if (skillVMFactory != null)
                        {
                            var skillVM = skillVMFactory.Create(child);
                            skillVM.OnDelete += Delete;
                            SkillVMs.Add(skillVM);
                        }
                    }
                }
                if(skill.References != null)
                {
                    foreach (var reference in skill.References)
                    {
                        var referenceVMFactory = _serviceProvider.GetRequiredService<ReferenceViewModelFactory>();
                        ReferenceUrlViewModel referenceVM = referenceVMFactory.Create(reference);
                        referenceVM.OnDelete += DeleteReference;
                        References.Add(referenceVM);
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
            if(skill != null)
            {
                dbContext.Skills.Remove(skill);
                dbContext.SaveChanges();
            }
        }

        void DeleteReference(ReferenceUrlViewModel referenceVM)
        {
            References.Remove(referenceVM);
        }

        void Delete(SkillViewModel skillVM)
        {
            skillVM.Dispose();
            SkillVMs.Remove(skillVM);
            RaisePropertyChanged(nameof(HasChildren));
        }

        public ICommand CreateReference
        {
            get => new UICommand((paremeter) =>
            {
                ReferenceUrl reference = new() { SkillId = Id };
                var db = _serviceProvider.GetRequiredService<MainDbContext>();
                db.Add<ReferenceUrl>(reference);
                db.SaveChanges();

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
                skill.ParentId = Id;

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

        public int Id { get; private set; }

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
        string _imagePath = "";
        public string ImagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                Update(skill=> skill.ImagePath = _imagePath);
                RaisePropertyChanged(nameof(ImagePath));
            }
        }

        public ObservableCollection<ReferenceUrlViewModel> References { get; set; } = new();

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

        public void Update (Action<Skill> setter)
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

        public ObservableCollection<SkillViewModel> SkillVMs { get; set; } = new();
    }
}
