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
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SkillBase.ViewModels
{
    internal class SkillsViewModel : BaseViewModel, IDisposable
    {
        IServiceProvider _serviceProvider;

        public SkillsViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task Init()
        {
            using var dbContext = _serviceProvider.GetRequiredService<MainDbContext>();
            if (dbContext == null) return; //TODO: error handling
            var skills = await dbContext.GetTreesAsync();

            SetLoadingView(true);

            ObservableCollection<SkillViewModel> _skillVMs = new();
            foreach (Skill skill in skills.OrderByDescending(x => x.CreatedAt))
            {
                var skillFactory = _serviceProvider?.GetRequiredService<SkillViewModelFactory>();
                var skillVM = skillFactory?.Create(skill, null);
                if (skillVM != null)
                {
                    skillVM.OnDelete += Delete;
                    _skillVMs.Add(skillVM);
                }
            }
            SkillVMs = _skillVMs;

            SetLoadingView(false);
        }
        public void Dispose()
        {
            foreach(var vm in _skillVMs)
            {
                vm.OnDelete -= Delete;
                vm.Dispose();
            }
        }

        bool ContainsChildRecursive(Skill skill, int childId)
        {
            if (skill.Children == null) return false;
            foreach (Skill child in skill.Children)
            {
                if (child.ParentId == childId || ContainsChildRecursive(child, childId))
                {
                    return true;
                }
            }
            return false;
        }

        void SetLoadingView(bool state)
        {
            IsLoaded = !state;
            if (state)
            {
                LoadingVisibility = Visibility.Visible;
                SkillsVisibility = Visibility.Collapsed;
            }
            else
            {
                LoadingVisibility = Visibility.Collapsed;
                SkillsVisibility = Visibility.Visible;
            }
        }

        bool _isLoaded = false;
        public bool IsLoaded
        {
            get => _isLoaded;
            set
            {
                _isLoaded = value;
                RaisePropertyChanged(nameof(IsLoaded));
            }
        }

        Visibility _loadingVisibility = Visibility.Visible;
        public Visibility LoadingVisibility
        {
            get => _loadingVisibility;
            set
            {
                _loadingVisibility = value;
                RaisePropertyChanged(nameof(LoadingVisibility));
            }
        }

        Visibility _skillsVisibility = Visibility.Collapsed;
        public Visibility SkillsVisibility
        {
            get => _skillsVisibility;
            set
            {
                _skillsVisibility = value;
                RaisePropertyChanged(nameof(SkillsVisibility));
            }
        }

        public LoadingUC Loading { get; set; } = new();

        ObservableCollection<SkillViewModel> _skillVMs = new();
        public ObservableCollection<SkillViewModel> SkillVMs
        {
            get => _skillVMs;
            set
            {
                _skillVMs = value;
                RaisePropertyChanged(nameof(SkillVMs));
            }
        }

        void Delete(SkillViewModel skillVM)
        {
            SkillVMs.Remove(skillVM);
            skillVM.OnDelete -= Delete;
            skillVM.Dispose();
        }

        public ICommand CreateSkill => new UICommand((parameter) =>
        {
            using var dbContext = _serviceProvider.GetRequiredService<MainDbContext>();
            Skill skill = new();
            dbContext.Skills.Add(skill);
            dbContext.SaveChanges();

            var skillVMFactory = _serviceProvider.GetRequiredService<SkillViewModelFactory>();
            var skillVM = skillVMFactory.Create(skill, null);
            skillVM.OnDelete += Delete;
            SkillVMs.Insert(0, skillVM);
        });

        internal void RemoveChild(SkillViewModel childVM)
        {
            childVM.OnDelete -= Delete;
            SkillVMs.Remove(childVM);
            childVM.SetParent(null);
        }
    }
}
