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
using System.Text;
using System.Threading.Tasks;

namespace SkillBase.ViewModels
{
    internal class SkillTreeViewModel : BaseViewModel
    {
        IServiceProvider _serviceProvider;

        public SkillTreeViewModel(IServiceProvider? serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task InitSkillTree()
        {
            using var dbContext = _serviceProvider?.GetRequiredService<MainDbContext>();
            if (dbContext == null) return; //TODO: error handling
            var skills = await dbContext.GetTreesAsync();
            foreach (Skill skill in skills)
            {
                var skillFactory = _serviceProvider?.GetRequiredService<SkillViewModelFactory>();
                var skillVM = skillFactory.Create(skill);
                skillVM.OnDelete += Delete;
                SkillVMs.Add(skillVM);
            }
        }
        public ObservableCollection<SkillViewModel> SkillVMs { get; set; } = new();

        void Delete(SkillViewModel skillVM)
        {
            SkillVMs.Remove(skillVM);
            skillVM.Dispose();
        }
    }
}
