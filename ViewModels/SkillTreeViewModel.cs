using Microsoft.Extensions.DependencyInjection;
using SkillBase.Data;
using SkillBase.Extensions;
using SkillBase.Models;
using SkillBase.ViewModels.Common;
using SkillBase.ViewModels.Factories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SkillBase.ViewModels
{
    internal class SkillTreeViewModel : BaseViewModel
    {
        IServiceProvider _serviceProvider;

        public SkillTreeViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task InitSkillTree()
        {
            using var dbContext = _serviceProvider.GetRequiredService<MainDbContext>();
            if (dbContext == null) return; //TODO: error handling
            var skills = await dbContext.GetTreesAsync();
            SkillVMs.Clear();
            foreach (Skill skill in skills)
            {
                var skillFactory = _serviceProvider?.GetRequiredService<SkillViewModelFactory>();
                var skillVM = skillFactory?.Create(skill, null);
                if (skillVM != null)
                {
                    //TODO: unsubscribe
                    skillVM.OnDelete += Delete;
                    SkillVMs.Add(skillVM);
                }
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

        public ObservableCollection<SkillViewModel> SkillVMs { get; set; } = new();

        void Delete(SkillViewModel skillVM)
        {
            SkillVMs.Remove(skillVM);
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
            SkillVMs.Add(skillVM);
        });

        internal void RemoveChild(SkillViewModel childVM)
        {
            SkillVMs.Remove(childVM);
            childVM.SetParent(null);
        }
    }
}
